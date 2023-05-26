# Documentação da API
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
