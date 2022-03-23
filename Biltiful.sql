
--use master
--drop database Biltiful

--Criação do Banco de Dados da Empresa Biltiful
CREATE DATABASE Biltiful
GO

USE Biltiful
GO

-- Criação das Tabelas Do Banco
CREATE TABLE Cliente(
CPF varchar(14) not null,
Nome varchar(50) not null,
DataNasc date not null,
sexo char not null,
UltimaCompra date not null DEFAULT getdate(),
DataCadastro date not null DEFAULT getdate(),
Situacao char not null,
CONSTRAINT pk_Cliente PRIMARY KEY (CPF)
)
GO

CREATE TABLE Fornecedor(
CNPJ varchar(18) not null,
RazaoSocial varchar(50) not null,
DataAbertura date not null,
UltimaCompra date not null DEFAULT getdate(),
DataCadastro date not null DEFAULT getdate(),
Situacao char not null,
CONSTRAINT pk_Fornecedor PRIMARY KEY (CNPJ)
)
GO

CREATE TABLE MateriaPrima(
Id varchar not null, 
Nome varchar(20) not null,
UltimaCompra date not null DEFAULT getdate(),
DataCadastro date not null DEFAULT getdate(),
Situacao char not null,
CONSTRAINT pk_MPrima PRIMARY KEY (Id)
)
GO

CREATE TABLE Produto(
CodEAN varchar(13) not null,
Nome varchar(20) not null,
ValorVenda decimal(5,2) not null,
UltimaVenda date not null DEFAULT getdate(),
DataCadastro date not null DEFAULT getdate(),
Situacao char not null,
CONSTRAINT pk_Produto PRIMARY KEY (CodEAN)
)
GO

CREATE TABLE Venda(
Id int identity(00001,1) not null,
DataVenda date not null DEFAULT getdate(),
Cliente varchar(14) not null,
ValorTotal decimal(7,2) null,
CONSTRAINT pk_Venda PRIMARY KEY (Id),
CONSTRAINT fk_ClienteVenda FOREIGN KEY (Cliente) REFERENCES Cliente (CPF)
)
GO

CREATE TABLE ItemVenda(
Id int not null,
Produto varchar(13) not null,
Quantidade decimal(4,2) not null,
ValorUnitario decimal(5,2) not null,
TotalItem decimal(6,2) null,
CONSTRAINT fk_Id_ItemVenda FOREIGN KEY (Id) REFERENCES Venda (Id),
CONSTRAINT fk_Prod_ItemVenda FOREIGN KEY (Produto) REFERENCES Produto (CodEAN)
)
GO

CREATE TABLE Compra(
Id int identity(00001,1) not null,
DataCompra date not null DEFAULT getdate(),
Fornecedor varchar(18) not null,
ValorTotal decimal(7,2) null,
CONSTRAINT pk_Compra PRIMARY KEY (Id),
CONSTRAINT fk_Fornecedor FOREIGN KEY (Fornecedor) REFERENCES Fornecedor(CNPJ)
)
GO

CREATE TABLE Item_Compra(
Id int not null,
Data date not null,
MateriaPrima varchar not null,
Qtd int not null,
ValorUnitario decimal(5,2) not null,
TotalItem decimal(6,2) null,
CONSTRAINT fk_Id_ItemCompra FOREIGN KEY (Id) REFERENCES Compra (Id),
CONSTRAINT fk_MP_itemCompra FOREIGN KEY (MateriaPrima) REFERENCES MateriaPrima (Id)
)
GO

CREATE TABLE Producao(
Id int identity (00001,1) not null,
DataProducao date not null DEFAULT getdate(),
Produto varchar(13),
Qtd decimal(5,2), 
CONSTRAINT pk_Producao PRIMARY KEY (Id),
CONSTRAINT fk_Producao FOREIGN KEY (Produto) REFERENCES Produto (CodEAN)
)
GO 

CREATE TABLE ItemProducao(
Id int not null,
DataProducao date not null,
MateriaPrima varchar not null,
QtdMateriaPrima decimal(5,2) not null,
CONSTRAINT fk_Id_itemProducao FOREIGN KEY (Id) REFERENCES Producao (Id),
CONSTRAINT fk_MP_itemProducao FOREIGN KEY (MateriaPrima) REFERENCES MateriaPrima (Id)
)
GO

--/////////////////////
--Procedures de Cliente
CREATE PROCEDURE proc_AdicionaCliente
	@CPF varchar(14),
	@Nome varchar(50),
	@DataNasc date,
	@Sexo char,
	@Situacao char
AS
BEGIN 
	INSERT INTO Cliente (CPF, Nome, DataNasc, sexo, Situacao)
	VALUES (@CPF, @Nome, @DataNasc, @Sexo, @Situacao)
END

CREATE PROCEDURE proc_SituacaoCliente
	@Situacao char,
	@CPF varchar(14)
AS
BEGIN
	UPDATE Cliente
	SET Situacao = @Situacao
	WHERE CPF = @CPF
END

CREATE PROCEDURE proc_EditaNome
	@CPF varchar(14),
	@Nome varchar(50)
	
AS
BEGIN
	UPDATE Cliente
	SET Nome = @Nome
	WHERE CPF = @CPF
END

CREATE PROCEDURE proc_EditaDataNasc
	@CPF varchar(14),
	@DataNasc date
AS
BEGIN
	UPDATE Cliente
	SET DataNasc = @DataNasc
	WHERE CPF = @CPF
END

CREATE PROCEDURE proc_EditaSexo
	@CPF varchar(14),
	@sexo char
AS
BEGIN
	UPDATE Cliente
	SET sexo = @sexo
	WHERE CPF = @CPF
END

--/////////////////////////////////////////////
-- Procedures de Fornecedor
CREATE PROCEDURE proc_AdicionaFornecedor
	@CNPJ varchar(18),
	@RazaoSocial varchar(50),
	@DataAbertura date,
	@Situacao char
AS
BEGIN 
	INSERT INTO Fornecedor(CNPJ, RazaoSocial, DataAbertura, Situacao)
	VALUES (@CNPJ, @RazaoSocial, @DataAbertura, @Situacao)
END

CREATE PROCEDURE proc_SituacaoFornecedor
	@Situacao char,
	@CNPJ varchar(14)
AS
BEGIN
	UPDATE Fornecedor
	SET Situacao = @Situacao
	WHERE CNPJ = @CNPJ
END

CREATE PROCEDURE proc_EditaRazaoSocial
	@CNPJ varchar(18),
	@RazaoSocial varchar(50)	
AS
BEGIN 
	UPDATE Fornecedor
	SET RazaoSocial = @RazaoSocial
	WHERE CNPJ = @CNPJ
END

CREATE PROCEDURE proc_EditaDataAbertura
	@CNPJ varchar(18),
	@DataAbertura varchar(50)	
AS
BEGIN 
	UPDATE Fornecedor
	SET DataAbertura = @DataAbertura
	WHERE CNPJ = @CNPJ
END





