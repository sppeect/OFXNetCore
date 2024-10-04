# OFXnet
Leitor de Open Financial Exchange Files (OFX)

O formato **OFX (Open Financial Exchange)** � baseado em XML e possui uma s�rie de tags que estruturam os dados financeiros dentro do arquivo. Embora existam muitas tags no OFX, as principais podem ser agrupadas em diferentes se��es, como transa��es banc�rias, investimentos, cart�es de cr�dito, etc. Abaixo est� uma descri��o das principais tags e seu prop�sito:

## Estrutura Geral do OFX
A estrutura de um arquivo OFX � dividida em v�rias partes, sendo as principais:

- **Cabe�alho OFX**: Inicia o arquivo OFX e especifica a vers�o e o tipo de codifica��o.
- **Transa��es banc�rias (Banking Transactions)**: Informa��es sobre a conta e as transa��es banc�rias.
- **Cart�es de cr�dito**: Informa��es sobre o cart�o de cr�dito e transa��es.
- **Investimentos**: Informa��es sobre investimentos e portf�lios.

---

## Cabe�alho do Arquivo OFX
O cabe�alho OFX � obrigat�rio e cont�m metadados sobre o arquivo:

- **`<OFXHEADER>`**: Define a vers�o do OFX (geralmente 100 ou 200).
- **`<DATA>`**: Especifica o tipo de dados, geralmente definido como `OFXSGML` para a vers�o 1.0 ou `OFXXML` para a vers�o 2.0.
- **`<VERSION>`**: Define a vers�o do protocolo OFX.
- **`<SECURITY>`**: Indica o tipo de seguran�a (normalmente "NONE").
- **`<ENCODING>`**: Define a codifica��o dos dados (geralmente `USASCII` ou `UTF-8`).
- **`<CHARSET>`**: Define o conjunto de caracteres usados no arquivo.
- **`<COMPRESSION>`**: Define se os dados est�o comprimidos.
- **`<OLDFILEUID>`**: Identificador �nico do arquivo anterior (se houver).
- **`<NEWFILEUID>`**: Identificador �nico do arquivo atual.

---

## Tags de Transa��es Banc�rias (Banking)
Essas tags s�o usadas para informa��es sobre contas banc�rias e transa��es:

- **`<BANKMSGSRSV1>`**: Se��o para mensagens relacionadas ao banco.
- **`<STMTTRNRS>`**: Usado para encapsular uma resposta de transa��o de extrato banc�rio.
- **`<STMTRS>`**: Cont�m os detalhes de um extrato banc�rio.
  - **`<CURDEF>`**: Define a moeda (por exemplo, USD).
  - **`<BANKACCTFROM>`**: Cont�m informa��es sobre a conta banc�ria de origem.
    - **`<BANKID>`**: N�mero do banco (ABA/Routing number nos EUA).
    - **`<ACCTID>`**: N�mero da conta.
    - **`<ACCTTYPE>`**: Tipo de conta (`CHECKING`, `SAVINGS`, etc.).
  - **`<BANKTRANLIST>`**: Lista de transa��es banc�rias.
    - **`<DTSTART>`**: Data de in�cio do per�odo de transa��es.
    - **`<DTEND>`**: Data de t�rmino do per�odo de transa��es.
    - **`<STMTTRN>`**: Representa uma transa��o �nica.
      - **`<TRNTYPE>`**: Tipo de transa��o (`CREDIT`, `DEBIT`, etc.).
      - **`<DTPOSTED>`**: Data de postagem da transa��o.
      - **`<TRNAMT>`**: Quantidade da transa��o (positivo para cr�ditos, negativo para d�bitos).
      - **`<FITID>`**: Identificador �nico da transa��o.
      - **`<CHECKNUM>`**: N�mero do cheque (se aplic�vel).
      - **`<MEMO>`**: Descri��o ou memo da transa��o.

---

## Tags de Cart�o de Cr�dito
As tags para cart�es de cr�dito s�o semelhantes �s banc�rias, com algumas diferen�as espec�ficas:

- **`<CREDITCARDMSGSRSV1>`**: Se��o para mensagens relacionadas a cart�es de cr�dito.
- **`<CCSTMTTRNRS>`**: Encapsula a resposta da transa��o do cart�o de cr�dito.
- **`<CCSTMTRS>`**: Cont�m os detalhes de um extrato de cart�o de cr�dito.
  - **`<CURDEF>`**: Define a moeda.
  - **`<CCACCTFROM>`**: Cont�m informa��es sobre a conta do cart�o de cr�dito.
    - **`<ACCTID>`**: N�mero da conta do cart�o de cr�dito.
  - **`<BANKTRANLIST>`**: Lista de transa��es de cart�o de cr�dito, semelhante �s transa��es banc�rias.

---

## Tags de Investimentos
Essas tags s�o usadas para informa��es sobre investimentos e portf�lios:

- **`<INVSTMTMSGSRSV1>`**: Se��o para mensagens relacionadas a investimentos.
- **`<INVSTMTTRNRS>`**: Encapsula a resposta da transa��o de investimento.
- **`<INVSTMTRS>`**: Cont�m os detalhes de um extrato de investimentos.
  - **`<DTSTART>`**: Data de in�cio do per�odo de transa��es.
  - **`<DTEND>`**: Data de t�rmino do per�odo de transa��es.
  - **`<INVPOSLIST>`**: Lista de posi��es de investimento.
    - **`<POSSTOCK>`**: Detalhes de uma posi��o em a��es.
    - **`<POSMF>`**: Detalhes de uma posi��o em fundos m�tuos.
    - **`<POSDEBT>`**: Detalhes de uma posi��o em t�tulos de d�vida.

---

## Outras Tags Comuns
- **`<SIGNONMSGSRSV1>`**: Se��o de mensagens de login e autentica��o.
  - **`<SONRS>`**: Resposta de login.
    - **`<STATUS>`**: Indica o status da solicita��o (por exemplo, sucesso ou erro).
      - **`<CODE>`**: C�digo de status.
      - **`<SEVERITY>`**: Severidade da mensagem (`INFO`, `ERROR`, etc.).
- **`<SIGNONMSGSRQV1>`**: Usado para enviar informa��es de login ao servidor.
  - **`<SONRQ>`**: Solicita��o de login.
