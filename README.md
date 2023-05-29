# Oracle
## Criar tabelas necessárias

Coloque seu login e senha em appsettings.json 

Passe esse código no oracle:

```
CREATE SEQUENCE SeqProcessos
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;
    
CREATE SEQUENCE SeqVistorias
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;
    
CREATE SEQUENCE SeqItens
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

CREATE SEQUENCE SeqRespostas
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

CREATE SEQUENCE SeqOpcoes
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;
    
CREATE TABLE Processos (
    IdProcesso NUMBER(10) DEFAULT SeqProcessos.NEXTVAL PRIMARY KEY,
    CodProcesso VARCHAR2(10),
    Descricao VARCHAR2(255),
    Ativo CHAR(1) DEFAULT 'S',
    DataDeCadastro DATE
);

CREATE TABLE Vistorias (
    IdVistoria NUMBER(10) DEFAULT SeqVistorias.NEXTVAL PRIMARY KEY,
    CodVistoria VARCHAR2(255),
    Descricao VARCHAR2(255),
    Processo VARCHAR2(10),
    Ativo CHAR(1) DEFAULT 'S',
    DataDeCadastro DATE
);

CREATE TABLE Itens (
    IdItem NUMBER(10) DEFAULT SeqItens.NEXTVAL PRIMARY KEY,
    Descricao VARCHAR2(255) NOT NULL,
    Ordem NUMBER(10) NOT NULL,
    Tipo CHAR(1) NOT NULL,
    Ativo CHAR(1) DEFAULT 'S',
    IdVistoria NUMBER(10) NOT NULL,
    FOREIGN KEY (IdVistoria) REFERENCES Vistorias (IdVistoria)
);

CREATE TABLE RespostasItens (
    IdResposta NUMBER(10) DEFAULT SeqRespostas.NEXTVAL PRIMARY KEY,
    Resposta VARCHAR2(255) NOT NULL,
    IdItem NUMBER(10) NOT NULL,
    FOREIGN KEY (IdItem) REFERENCES Itens (IdItem)
);

CREATE TABLE OpcoesItens (
    IdOpcao NUMBER(10) DEFAULT SeqOpcoes.NEXTVAL PRIMARY KEY,
    Opcao VARCHAR2(255) NOT NULL,
    IdItem NUMBER(10) NOT NULL,
    FOREIGN KEY (IdItem) REFERENCES Itens (IdItem)
);
```

# API

## Itens
### GET /api/Itens
Recupera uma lista de itens.

### POST /api/Itens
Cria um novo item.

**Body**:

```
[
  {
    "idItem": 0,
    "descricao": "string",
    "ordem": 0,
    "tipo": 0,
    "idVistoria": 0,
    "opcaoModels": [
      {
        "idItemList": [
          0
        ],
        "opcoes": [
          {
            "idOpcao": 0,
            "opcao": "string",
            "idItem": 0
          }
        ]
      }
    ],
    "respostaModels": [
      {
        "idItemList": [
          0
        ],
        "respostas": [
          {
            "idResposta": 0,
            "resposta": "string",
            "idItem": 0
          }
        ]
      }
    ]
  }
]
```
### PUT /api/Itens/{idItem}
Atualiza um item.

Parâmetros:

**idItem: int**

**Body**:
```
{
  "idItem": 0,
  "descricao": "string",
  "ordem": 0,
  "tipo": 0,
  "idVistoria": 0,
  "opcaoModels": [
    {
      "idItemList": [
         0
      ],
      "opcoes": [
        {
          "idOpcao": 0,
          "opcao": "string",
          "idItem": 0
        }
      ]
    }
  ],
  "respostaModels": [
    {
      "idItemList": [
        0
      ],
      "respostas": [
        {
          "idResposta": 0,
          "resposta": "string",
          "idItem": 0
        }
      ]
    }
  ]
}
```

### DELETE /api/Itens/{idItem}
Exclui um item.

Parâmetros:

**idItem: int**
## Opcoes

### GET /api/Opcoes
Recupera uma lista de opções.

### POST /api/Opcoes
Cria uma nova opção.

**Body:**
```
[
  {
    "idOpcao": 0,
    "opcao": "string",
    "idItem": 0
  }
]
```
## Processos

### GET /api/Processos
Recupera uma lista de processos.

### POST /api/Processos
Cria um novo processo.

**Body:**
```
{
  "idProcesso": 0,
  "codProcesso": "string",
  "descricao": "string",
  "dataDeCadastro": "2023-05-26T20:35:21.796Z"
}
```

### PUT /api/Processos/{idProcesso}
Atualiza um processo.

Parâmetros:

**idProcesso: int**

**Body:**
```
{
  "idProcesso": 0,
  "codProcesso": "string",
  "descricao": "string",
  "dataDeCadastro": "2023-05-26T20:35:21.797Z"
}
```
Respostas
200: Sucesso.

### DELETE /api/Processos/{idProcesso}
Exclui um processo.

Parâmetros:

**idProcesso: int**
## Respostas

### GET /api/Respostas
Recupera uma lista de respostas.

### POST /api/Respostas
Cria uma nova resposta.

Parâmetros:

**idItem: int**

**Body:**
```
[
  {
    "idResposta": 0,
    "resposta": "string",
    "idItem": 0
  }
]
```
Respostas
200: Sucesso.
## Vistorias

### GET /api/Vistorias
Recupera uma lista de vistorias.

### POST /api/Vistorias
Cria uma nova vistoria.

**Body:**
```
{
  "idVistoria": 0,
  "codVistoria": "string",
  "descricao": "string",
  "processo": "string",
  "dataDeCadastro": "2023-05-26T20:35:21.803Z",
  "itens": [
    {
      "idItem": 0,
      "descricao": "string",
      "ordem": 0,
      "tipo": 0,
      "idVistoria": 0,
      "opcaoModels": [
        {
          "idItemList": [
            0
          ],
          "opcoes": [
            {
              "idOpcao": 0,
              "opcao": "string",
              "idItem": 0
            }
          ]
        }
      ],
      "respostaModels": [
        {
          "idItemList": [
            0
          ],
          "respostas": [
            {
              "idResposta": 0,
              "resposta": "string",
              "idItem": 0
            }
          ]
        }
      ]
    }
  ]
}
```
Respostas
200: Sucesso.

### PUT /api/Vistorias/{idVistoria}
Atualiza uma vistoria.

Parâmetros:

**idVistoria: int**

**Body:**
```
{
  "idVistoria": 0,
  "codVistoria": "string",
  "descricao": "string",
  "processo": "string",
  "dataDeCadastro": "2023-05-26T20:35:21.807Z",
  "itens": [
    {
      "idItem": 0,
      "descricao": "string",
      "ordem": 0,
      "tipo": 0,
      "idVistoria": 0,
      "opcaoModels": [
        {
          "idItemList": [
            0
          ],
          "opcoes": [
            {
              "idOpcao": 0,
              "opcao": "string",
              "idItem": 0
            }
          ]
        }
      ],
      "respostaModels": [
        {
          "idItemList": [
            0
          ],
          "respostas": [
            {
              "idResposta": 0,
              "resposta": "string",
              "idItem": 0
            }
          ]
        }
      ]
    }
  ]
}
```

### DELETE /api/Vistorias/{idVistoria}
Exclui uma vistoria.

Parâmetros:

**idVistoria: int**
