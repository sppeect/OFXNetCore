# OFXnetCore
Leitor de Open Financial Exchange Files (OFX)

O formato **OFX (Open Financial Exchange)** é baseado em XML e possui uma série de tags que estruturam os dados financeiros dentro do arquivo. Embora existam muitas tags no OFX, as principais podem ser agrupadas em diferentes seções, como transações bancárias, investimentos, cartões de crédito, etc. Abaixo está uma descrição das principais tags e seu propósito:

## Estrutura Geral do OFX
A estrutura de um arquivo OFX é dividida em várias partes, sendo as principais:

- **Cabeçalho OFX**: Inicia o arquivo OFX e especifica a versão e o tipo de codificação.
- **Transações bancárias (Banking Transactions)**: Informações sobre a conta e as transações bancárias.
- **Cartões de crédito**: Informações sobre o cartão de crédito e transações.
- **Investimentos**: Informações sobre investimentos e portfólios.

---

## Cabeçalho do Arquivo OFX
O cabeçalho OFX é obrigatório e contém metadados sobre o arquivo:

- **`<OFXHEADER>`**: Define a versão do OFX (geralmente 100 ou 200).
- **`<DATA>`**: Especifica o tipo de dados, geralmente definido como `OFXSGML` para a versão 1.0 ou `OFXXML` para a versão 2.0.
- **`<VERSION>`**: Define a versão do protocolo OFX.
- **`<SECURITY>`**: Indica o tipo de segurança (normalmente "NONE").
- **`<ENCODING>`**: Define a codificação dos dados (geralmente `USASCII` ou `UTF-8`).
- **`<CHARSET>`**: Define o conjunto de caracteres usados no arquivo.
- **`<COMPRESSION>`**: Define se os dados estão comprimidos.
- **`<OLDFILEUID>`**: Identificador único do arquivo anterior (se houver).
- **`<NEWFILEUID>`**: Identificador único do arquivo atual.

---

## Tags de Transações Bancárias (Banking)
Essas tags são usadas para informações sobre contas bancárias e transações:

- **`<BANKMSGSRSV1>`**: Seção para mensagens relacionadas ao banco.
- **`<STMTTRNRS>`**: Usado para encapsular uma resposta de transação de extrato bancário.
- **`<STMTRS>`**: Contém os detalhes de um extrato bancário.
  - **`<CURDEF>`**: Define a moeda (por exemplo, USD).
  - **`<BANKACCTFROM>`**: Contém informações sobre a conta bancária de origem.
    - **`<BANKID>`**: Número do banco (ABA/Routing number nos EUA).
    - **`<ACCTID>`**: Número da conta.
    - **`<ACCTTYPE>`**: Tipo de conta (`CHECKING`, `SAVINGS`, etc.).
  - **`<BANKTRANLIST>`**: Lista de transações bancárias.
    - **`<DTSTART>`**: Data de início do período de transações.
    - **`<DTEND>`**: Data de término do período de transações.
    - **`<STMTTRN>`**: Representa uma transação única.
      - **`<TRNTYPE>`**: Tipo de transação (`CREDIT`, `DEBIT`, etc.).
      - **`<DTPOSTED>`**: Data de postagem da transação.
      - **`<TRNAMT>`**: Quantidade da transação (positivo para créditos, negativo para débitos).
      - **`<FITID>`**: Identificador único da transação.
      - **`<CHECKNUM>`**: Número do cheque (se aplicável).
      - **`<MEMO>`**: Descrição ou memo da transação.

---

## Tags de Cartão de Crédito
As tags para cartões de crédito são semelhantes às bancárias, com algumas diferenças específicas:

- **`<CREDITCARDMSGSRSV1>`**: Seção para mensagens relacionadas a cartões de crédito.
- **`<CCSTMTTRNRS>`**: Encapsula a resposta da transação do cartão de crédito.
- **`<CCSTMTRS>`**: Contém os detalhes de um extrato de cartão de crédito.
  - **`<CURDEF>`**: Define a moeda.
  - **`<CCACCTFROM>`**: Contém informações sobre a conta do cartão de crédito.
    - **`<ACCTID>`**: Número da conta do cartão de crédito.
  - **`<BANKTRANLIST>`**: Lista de transações de cartão de crédito, semelhante às transações bancárias.

---

## Tags de Investimentos
Essas tags são usadas para informações sobre investimentos e portfólios:

- **`<INVSTMTMSGSRSV1>`**: Seção para mensagens relacionadas a investimentos.
- **`<INVSTMTTRNRS>`**: Encapsula a resposta da transação de investimento.
- **`<INVSTMTRS>`**: Contém os detalhes de um extrato de investimentos.
  - **`<DTSTART>`**: Data de início do período de transações.
  - **`<DTEND>`**: Data de término do período de transações.
  - **`<INVPOSLIST>`**: Lista de posições de investimento.
    - **`<POSSTOCK>`**: Detalhes de uma posição em ações.
    - **`<POSMF>`**: Detalhes de uma posição em fundos mútuos.
    - **`<POSDEBT>`**: Detalhes de uma posição em títulos de dívida.

---

## Outras Tags Comuns
- **`<SIGNONMSGSRSV1>`**: Seção de mensagens de login e autenticação.
  - **`<SONRS>`**: Resposta de login.
    - **`<STATUS>`**: Indica o status da solicitação (por exemplo, sucesso ou erro).
      - **`<CODE>`**: Código de status.
      - **`<SEVERITY>`**: Severidade da mensagem (`INFO`, `ERROR`, etc.).
- **`<SIGNONMSGSRQV1>`**: Usado para enviar informações de login ao servidor.
  - **`<SONRQ>`**: Solicitação de login.
