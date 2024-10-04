using OFXnet.Core.Exceptions;
using OFXnet.Domain;
using OFXnet.Domain.Transactions;
using OFXnet.Shared;
using System.Text;
using System.Xml;

namespace OFXnet.Core
{
    public static class Parser
    {
        #region Public methods

        /// <summary>
        /// Extract object with OFX file data. This method checks the OFX file.
        /// </summary>
        /// <param name="ofxSourceFile">Full path of OFX file</param>
        /// <returns>Extract object with OFX file data.</returns>
        public static Extract GenerateExtract(string ofxSourceFile)
        {
            return GenerateExtract(ofxSourceFile, new ParserSettings());
        }

        public static Extract GenerateExtract(string ofxSourceFile, ParserSettings settings)
        {
            ExportOfxToXml(ofxSourceFile, ofxSourceFile + ".xml");

            XmlTextReader xmlTextReader = new XmlTextReader(ofxSourceFile + ".xml");

            return GetExtractByXmlExported(xmlTextReader, settings);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method translate an OFX file to XML tags, independent of the content.
        /// </summary>
        /// <param name="ofxSourceFile">OFX source file</param>
        /// <returns>XML tags in StringBuilder object.</returns>
        private static StringBuilder TranslateOfxToXml(string ofxSourceFile)
        {
            StringBuilder result = new StringBuilder();
            int level = 0;
            string line;

            if (!File.Exists(ofxSourceFile))
                throw new FileNotFoundException("OFX source file not found: " + ofxSourceFile);

            StreamReader streamReader = File.OpenText(ofxSourceFile);
            while ((line = streamReader.ReadLine()) != null)
            {
                line = line.Trim();

                if (line.StartsWith("</") && line.EndsWith(">"))
                {
                    AddTabs(result, level, true);
                    level--;
                    result.Append(line);
                }
                else if (line.StartsWith("<") && line.EndsWith(">"))
                {
                    if (line == "<BALAMT>" || line == "<PRINYTD>" || line == "<PRINLTD>")
                    {
                        AddTabs(result, level + 1, true);
                        result.Append(line);
                        result.Append(ReturnFinalTag(line));
                    }
                    else
                    {
                        level++;
                        AddTabs(result, level, true);
                        result.Append(line);
                    }
                }
                else if (line.StartsWith("<") && !line.EndsWith(">"))
                {
                    AddTabs(result, level + 1, true);
                    result.Append(line);
                    result.Append(ReturnFinalTag(line));
                }
            }
            streamReader.Close();

            return result;
        }

        private static Extract GetExtractByXmlExported(XmlTextReader xmlTextReader, ParserSettings settings)
        {
            if (settings == null)
                settings = new ParserSettings();

            string currentElement = string.Empty;
            Transaction currentTransaction = null;

            // Variáveis utilizadas para ler o XML
            HeaderExtract header = new HeaderExtract();
            BankAccount bankAccount = new BankAccount();
            Extract extract = new Extract(header, bankAccount, "");

            bool hasHeader = false;
            bool hasAccountInfoData = false;

            try
            {
                while (xmlTextReader.Read())
                {
                    if (xmlTextReader.NodeType == XmlNodeType.EndElement)
                    {
                        switch (xmlTextReader.Name)
                        {
                            case "STMTTRN":
                            case "CCSTMTTRNRS":
                            case "INVSTMTTRNRS":
                                if (currentTransaction != null)
                                {
                                    extract.AddTransaction(currentTransaction);
                                    currentTransaction = null;
                                }
                                break;
                        }
                    }

                    if (xmlTextReader.NodeType == XmlNodeType.Element)
                    {
                        currentElement = xmlTextReader.Name;

                        switch (currentElement)
                        {
                            case "CREDITCARDMSGSRSV1":
                                // Inicializar a seção de cartão de crédito
                                // Pode conter várias transações
                                break;

                            case "INVSTMTMSGSRSV1":
                                // Inicializar a seção de investimentos
                                // Pode conter várias transações de investimento
                                break;

                            case "STMTTRN":
                                currentTransaction = new BankTransaction("", "", DateTime.MinValue, 0, "", 0);
                                break;

                            case "CCSTMTTRNRS":
                                currentTransaction = new CreditCardTransaction("", "", DateTime.MinValue, 0, "", "");
                                break;

                            case "INVSTMTTRNRS":
                                currentTransaction = new InvestmentTransaction("", "", DateTime.MinValue, 0, "", "", 0, 0);
                                break;
                        }
                    }

                    if (xmlTextReader.NodeType == XmlNodeType.Text)
                    {
                        switch (currentElement)
                        {
                            // Informações de cabeçalho
                            case "DTSERVER":
                                header.ServerDate = ConvertOfxDateToDateTime(xmlTextReader.Value, extract);
                                hasHeader = true;
                                break;
                            case "LANGUAGE":
                                header.Language = xmlTextReader.Value;
                                hasHeader = true;
                                break;
                            case "ORG":
                                header.BankName = xmlTextReader.Value;
                                hasHeader = true;
                                break;

                            // Informações de conta de cartão de crédito
                            case "CURDEF":
                                if (currentTransaction is CreditCardTransaction creditTransaction)
                                {
                                    // Atribuir o valor da moeda para a transação de cartão de crédito
                                    // currency = xmlTextReader.Value;
                                }
                                break;

                            // Informações de conta
                            case "BANKID":
                                bankAccount.Bank = new Bank(TryGetBankId(xmlTextReader.Value, extract), "");
                                hasAccountInfoData = true;
                                break;
                            case "BRANCHID":
                                bankAccount.AgencyCode = xmlTextReader.Value;
                                hasAccountInfoData = true;
                                break;
                            case "ACCTID":
                                bankAccount.AccountCode = xmlTextReader.Value;
                                hasAccountInfoData = true;
                                break;
                            case "ACCTTYPE":
                                bankAccount.Type = xmlTextReader.Value;
                                hasAccountInfoData = true;
                                break;

                            // Detalhes da transação
                            case "TRNTYPE":
                                currentTransaction.Type = xmlTextReader.Value;
                                break;
                            case "DTPOSTED":
                                currentTransaction.Date = ConvertOfxDateToDateTime(xmlTextReader.Value, extract);
                                break;
                            case "TRNAMT":
                                currentTransaction.TransactionValue = GetTransactionValue(xmlTextReader.Value, extract, settings);
                                break;
                            case "FITID":
                                currentTransaction.Id = xmlTextReader.Value;
                                break;
                            case "MEMO":
                                currentTransaction.Description = xmlTextReader.Value;
                                currentTransaction.SenderDocument = currentTransaction.ExtractDocumentFromDescription(xmlTextReader.Value);
                                break;
                            case "DTSTART":
                                extract.InitialDate = ConvertOfxDateToDateTime(xmlTextReader.Value, extract);
                                break;
                            case "DTEND":
                                extract.FinalDate = ConvertOfxDateToDateTime(xmlTextReader.Value, extract);
                                break;

                            // Campos específicos de transações bancárias
                            case "CHECKNUM":
                                if (currentTransaction is BankTransaction bankTransaction)
                                {
                                    bankTransaction.Checksum = Convert.ToInt64(xmlTextReader.Value);
                                }
                                break;

                            // Campos específicos de transações de cartão de crédito
                            case "CARDNUM":
                                if (currentTransaction is CreditCardTransaction creditCardTransaction)
                                {
                                    creditCardTransaction.CardNumber = xmlTextReader.Value;
                                }
                                break;

                            // Campos específicos de transações de investimentos
                            case "INVESTMENTTYPE":
                                if (currentTransaction is InvestmentTransaction investmentTransaction)
                                {
                                    investmentTransaction.InvestmentType = xmlTextReader.Value;
                                }
                                break;
                            case "QUANTITY":
                                if (currentTransaction is InvestmentTransaction investmentTransactionQty)
                                {
                                    investmentTransactionQty.Quantity = Convert.ToDouble(xmlTextReader.Value);
                                }
                                break;
                            case "PRICE":
                                if (currentTransaction is InvestmentTransaction investmentTransactionPrice)
                                {
                                    investmentTransactionPrice.Price = Convert.ToDouble(xmlTextReader.Value);
                                }
                                break;
                        }
                    }
                }
            }
            catch (XmlException xe)
            {
                throw new OFXParserException($"Invalid OFX file! Internal message: {xe.Message}");
            }
            finally
            {
                xmlTextReader.Close();
            }

            if ((settings.IsValidateHeader && !hasHeader) ||
                (settings.IsValidateAccountData && !hasAccountInfoData))
            {
                throw new OFXParserException("Invalid OFX file!");
            }

            return extract;
        }



        /// <summary>
        /// This method translate an OFX file to XML file, independent of the content.
        /// </summary>
        /// <param name="ofxSourceFile">Path of OFX source file</param>
        /// <param name="xmlNewFile">Path of the XML file, internally generated.</param>
        private static void ExportOfxToXml(string ofxSourceFile, string xmlNewFile)
        {
            if (!File.Exists(ofxSourceFile))
            {
                throw new FileNotFoundException(string.Format(Constants.MESSAGE_OFX_FILE_NOT_FOUND, ofxSourceFile));
            }

            if (!xmlNewFile.ToLower().EndsWith(".xml"))
            {
                throw new ArgumentException(string.Format(Constants.MESSAGE_NAME_NEW_FILE_INVALID, xmlNewFile));
            }

            StringBuilder ofxTranslated = TranslateOfxToXml(ofxSourceFile);

            WriteCreatedXmlIntoTargetInternalFile(xmlNewFile, ofxTranslated);
        }

        private static void WriteCreatedXmlIntoTargetInternalFile(string xmlNewFile, StringBuilder ofxTranslated)
        {
            // Verifying if target file exists
            if (File.Exists(xmlNewFile))
            {
                File.Delete(xmlNewFile);
            }

            StreamWriter sw = File.CreateText(xmlNewFile);
            sw.WriteLine(@"<?xml version=""1.0""?>");
            sw.WriteLine(ofxTranslated.ToString());
            sw.Close();
        }

        /// <summary>
        /// This method return the correct closing tag string 
        /// </summary>
        /// <param name="content">Content of analysis</param>
        /// <returns>String with ending tag.</returns>
        private static string ReturnFinalTag(string content)
        {
            string finalTagReturn = string.Empty;

            if (content.IndexOf("<") != -1 && content.IndexOf(">") != -1)
            {
                int position1 = content.IndexOf("<");
                int position2 = content.IndexOf(">");
                if (position2 - position1 > 2)
                {
                    finalTagReturn = content.Substring(position1, position2 - position1 + 1);
                    finalTagReturn = finalTagReturn.Replace("<", "</");
                }
            }
            return finalTagReturn;
        }

        /// <summary>
        /// This method add tabs into lines of xml file, to best identation.
        /// </summary>
        /// <param name="stringObject">Line of content</param>
        /// <param name="lengthTabs">Length os tabs to add into content</param>
        /// <param name="newLine">Is it new line?</param>
        private static void AddTabs(StringBuilder stringObject, int lengthTabs, bool newLine)
        {
            if (newLine)
                stringObject.AppendLine();

            for (int j = 1; j < lengthTabs; j++)
            {
                stringObject.Append("\t");
            }
        }

        /// <summary>
        /// Method that return a part of date. Is is used internally when the dates are reading.
        /// </summary>
        /// <param name="ofxDate">Date</param>
        /// <param name="partDateTime">Part of date</param>
        /// <returns></returns>
        private static int GetPartOfOfxDate(string ofxDate, PartDateTime partDateTime)
        {
            if (partDateTime == PartDateTime.YEAR)
            {
                return int.Parse(ofxDate.Substring(0, 4));

            }
            else if (partDateTime == PartDateTime.MONTH)
            {
                return int.Parse(ofxDate.Substring(4, 2));

            }
            else if (partDateTime == PartDateTime.DAY)
            {
                return int.Parse(ofxDate.Substring(6, 2));

            }
            else if (partDateTime == PartDateTime.HOUR && ofxDate.Length >= 10)
            {
                return int.Parse(ofxDate.Substring(8, 2));

            }
            else if (partDateTime == PartDateTime.MINUTE && ofxDate.Length >= 12)
            {
                return int.Parse(ofxDate.Substring(10, 2));

            }
            else if (partDateTime == PartDateTime.SECOND && ofxDate.Length >= 14)
            {
                return int.Parse(ofxDate.Substring(12, 2));

            }
            return 0;
        }

        /// <summary>
        /// Method that convert a OFX date string to DateTime object.
        /// </summary>
        /// <param name="ofxDate"></param>
        /// <param name="extract"></param>
        /// <returns></returns>
        private static DateTime ConvertOfxDateToDateTime(string ofxDate, Extract extract)
        {
            DateTime dateTimeReturned = DateTime.MinValue;
            try
            {
                int year = GetPartOfOfxDate(ofxDate, PartDateTime.YEAR);
                int month = GetPartOfOfxDate(ofxDate, PartDateTime.MONTH);
                int day = GetPartOfOfxDate(ofxDate, PartDateTime.DAY);
                int hour = GetPartOfOfxDate(ofxDate, PartDateTime.HOUR);
                int minute = GetPartOfOfxDate(ofxDate, PartDateTime.MINUTE);
                int second = GetPartOfOfxDate(ofxDate, PartDateTime.SECOND);

                dateTimeReturned = new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception)
            {
                extract.ImportingErrors.Add(string.Format(Constants.MESSAGE_INVALID_DATETIME, ofxDate));
            }
            return dateTimeReturned;
        }

        private static int TryGetBankId(string value, Extract extract)
        {
            if (int.TryParse(value, out int bankId))
                return bankId;

            extract.ImportingErrors.Add(string.Format(Constants.MESSAGE_BANK_ID_ISNT_NUMERIC_VALUE, value));
            return 0;
        }

        private static double GetTransactionValue(string value, Extract extract, ParserSettings settings)
        {
            try
            {
                if (settings.CustomConverterCurrency != null)
                    return settings.CustomConverterCurrency.Invoke(value);
                else
                    return Convert.ToDouble(value.Replace('.', ','));
            }
            catch (Exception)
            {
                extract.ImportingErrors.Add(string.Format(Constants.MESSAGE_INVALID_TRANSACTION_VALUE_AMOUNT, value));
            }
            return 0;
        }

        #endregion
    }
}
