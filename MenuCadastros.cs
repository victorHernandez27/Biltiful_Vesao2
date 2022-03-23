using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biltiful_Vesao2
{

    public class MenuCadastros
    {
        public class Cliente
        {
            public string CPF { get; private set; }
            public string Nome { get; set; }
            public DateTime DataNascimento { get; set; }
            public char Sexo { get; set; }
            public DateTime UltimaVenda { get; set; }
            public DateTime DataCadastro { get; set; }
            public char Situacao { get; set; }

            public Cliente()
            {

            } //construtor vazio pra chamar ele sem ter que passar parametros

            public Cliente(string cpf, string name, DateTime dataNascimento, char sexo, char situacao) //Construtor com os dados que tem que informar pra cadastrar
            {
                CPF = cpf;
                Nome = name;
                DataNascimento = dataNascimento;
                Sexo = sexo;
                UltimaVenda = DateTime.Now;
                DataCadastro = DateTime.Now;
                Situacao = situacao;
            }
            public Cliente(string cpf, string name, DateTime dataNascimento, char sexo, DateTime UltimaCompra, DateTime dataCadastro, char situacao) //construtor criado só pra ocupar espaço no codigo, pq nao usa pra nada.
            {
                CPF = cpf;
                Nome = name;
                DataNascimento = dataNascimento;
                Sexo = sexo;
                UltimaVenda = UltimaCompra;
                DataCadastro = dataCadastro;
                Situacao = situacao;
            }
            public override string ToString()
            {
                return $"CPF: {CPF}\nNome: {Nome.Trim()}\nData de nascimento: {DataNascimento.ToString("dd/MM/yyyy")}\nSexo: {Sexo}\nUltima Compra: {UltimaVenda.ToString("dd/MM/yyyy")}\nDia de Cadastro: {DataCadastro.ToString("dd/MM/yyyy")}\nSituacao: {Situacao}";
            }

        }
        public static void SubMenuClientes()
        {
            string escolha;

            do
            {
                Console.Clear();

                Console.WriteLine("=============== CLIENTES ===============");
                Console.WriteLine("1. Cadastar cliente");
                Console.WriteLine("2. Listar clientes");
                Console.WriteLine("3. Editar registro de cliente");
                Console.WriteLine("4. Bloquear/Desbloqueia cliente (Inadimplente)");
                Console.WriteLine("5. Localizar cliente");
                Console.WriteLine("6. Localizar cliente bloqueado");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("0. Voltar ao menu anterior");
                Console.Write("\nEscolha: ");

                switch (escolha = Console.ReadLine())
                {
                    case "0":
                        break;

                    case "1":
                        NovoCliente();
                        break;

                    case "2":
                        ListarClientes();
                        break;

                    case "3":
                        EditarCliente();
                        break;

                    case "4":
                        BloqueiaCliente();

                        break;

                    case "5":
                        LocalizaCliente();
                        break;

                    case "6":
                        LocalizaClienteBloqueado();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida");
                        Console.WriteLine("\n Pressione ENTER para voltar ao menu");
                        break;
                }

            } while (escolha != "0");

        }
        public static void NovoCliente()
        {
            Console.Clear();


            Console.WriteLine("Informe a data de nascimento:");
            DateTime dNascimento = DateTime.Parse(Console.ReadLine());
            DateTime DataAtual = DateTime.Now;

            var idade = DateTime.Today.Year - dNascimento.Year;
            if (DateTime.Today.DayOfYear < dNascimento.DayOfYear)
                idade = idade - 1;

            if (idade >= 18)
            {
                string cpf, nome;
                char situacao, sexo;

                do
                {
                    Console.Write("CPF: ");
                    cpf = Console.ReadLine();
                    cpf = cpf.Trim();
                    cpf = cpf.Replace(".", "").Replace("-", "");

                    if (Validacoes.ValidarCpf(cpf) == false)
                        Console.WriteLine("CPF Inválido. Digite um CPF Válido, ou 0 para sair.");
                } while (Validacoes.ValidarCpf(cpf) == false && cpf != "0");
                if (cpf == "0")
                    return;
                else
                {
                    Console.Write("Nome: ");
                    nome = Console.ReadLine();
                    Console.Write("Genero (M - Masculino/ F - Feminino): ");
                    sexo = char.Parse(Console.ReadLine().ToUpper());
                    Console.Write("Situacao (A - Ativo/ I - Inativo): ");
                    situacao = char.Parse(Console.ReadLine().ToUpper());
                }

                try
                {
                    var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
                    SqlConnection cnn = new SqlConnection(connetionString);
                    using (cnn)
                    {

                        cnn.Open();
                        SqlCommand sql_cmnd = new SqlCommand("proc_AdicionaCliente", cnn);

                        sql_cmnd.CommandType = CommandType.StoredProcedure;

                        sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                        sql_cmnd.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = nome;
                        sql_cmnd.Parameters.AddWithValue("@DataNasc", SqlDbType.Date).Value = dNascimento;
                        sql_cmnd.Parameters.AddWithValue("@sexo", SqlDbType.Char).Value = sexo;
                        sql_cmnd.Parameters.AddWithValue("@Situacao", SqlDbType.Char).Value = situacao;
                        sql_cmnd.ExecuteNonQuery();
                        cnn.Close();
                        Console.WriteLine("Cliente incluido no banco de dados.");
                        Console.ReadKey();


                    }
                }
                catch (SqlException erro)
                {
                    Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                    Console.ReadKey();
                }
                return;

            }
            else
            {
                Console.WriteLine("Menor de 18 anos nao pode ser cadastrado. Presione ENTER para voltar ao menu...");
                Console.ReadKey();
            }
        }
        public static void ListarClientes()
        {
            List<string> list = new List<string>();

            var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            string Select = $"SELECT Nome, CPF, format(DataNasc,'dd/MM/yyyy'), sexo, Situacao FROM dbo.Cliente";
            using (SqlCommand comando = new SqlCommand(Select, cnn))
            {
                using (SqlDataReader Ler = comando.ExecuteReader())
                {
                    if (Ler.HasRows == true)
                    {
                        while (Ler.Read())
                        {
                            list.Add($"Nome: {Ler.GetString(0)}\nCPF: {Ler.GetString(1)}\nData de Nascimento: {Ler.GetString(2)}\nSexo:{Ler.GetString(3)}\nSituação: {Ler.GetString(4)}\n");
                        }
                        cnn.Close();
                        list.Sort();
                        int i = 0;
                        string opcao;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine(list[i]);
                            Console.WriteLine("\n\n1 - Proximo\t2 - Anterior\t3 - Primeiro\t4 - Ultimo\t0 - Sair");
                            opcao = Console.ReadLine();
                            switch (opcao)
                            {
                                case "1":
                                    if (i == list.Count() - 1)
                                        i = list.Count() - 1;
                                    else
                                        i++;
                                    break;

                                case "2":
                                    if (i == 0)
                                        i = 0;
                                    else
                                        i--;
                                    break;

                                case "3":
                                    i = 0;
                                    break;

                                case "4":
                                    i = list.Count() - 1;
                                    break;

                                case "0":
                                    break;

                                default:
                                    break;
                            }
                        }
                        while (opcao != "0");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Não existem clientes cadastrados.");
                        Console.ReadKey();
                    }
                }
            }
        }
        public static void BloqueiaCliente()
        {

            Console.WriteLine("Insira o CPF para bloqueio: ");
            string cpf = Console.ReadLine();
            cpf = cpf.Replace(".", "").Replace("-", "");
            Console.Write("Informe a situação do cliente (A - Ativo/ I - Inativo): ");
            char situacao = char.Parse(Console.ReadLine().ToUpper());
            try
            {
                var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
                SqlConnection cnn = new SqlConnection(connetionString);
                using (cnn)
                {

                    cnn.Open();
                    SqlCommand sql_cmnd = new SqlCommand("proc_SituacaoCliente", cnn);

                    sql_cmnd.CommandType = CommandType.StoredProcedure;

                    sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                    sql_cmnd.Parameters.AddWithValue("@Situacao", SqlDbType.Char).Value = situacao;
                    sql_cmnd.ExecuteNonQuery();
                    cnn.Close();
                    Console.WriteLine("Situação do cliente alterada para .'" + situacao + "'\nPressione ENTER para voltar ao menu...");
                    Console.ReadKey();


                }
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }        
        public static void EditarCliente()
        {
            Console.WriteLine("Informe o CPF do cliente que deseja alterar:");
            var cpf = Console.ReadLine().Replace(".", "").Replace("-", "");

            Console.Clear();
            Console.WriteLine("Informe qual dado deseja alterar do cliente:");
            Console.WriteLine("1. Nome\n2. Data de Nascimento\n3. Sexo");

            var escolha = Console.ReadLine();
            string novoNome;
            DateTime novaData;
            char novoSexo;

            try
            {
                var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
                SqlConnection cnn = new SqlConnection(connetionString);
                SqlCommand sql_cmnd;
                using (cnn)
                {
                    do
                    {

                        switch (escolha)
                        {
                            case "1":
                                Console.WriteLine("Digite o Nome para alterar");
                                novoNome = Console.ReadLine();
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("proc_EditaNome", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                                sql_cmnd.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = novoNome;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();

                                Console.WriteLine("Deseja alterar mais alguma informação do Cliente?:");
                                Console.WriteLine("\n1. Nome\n2. Data de Nascimento\n3. Sexo\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "2":
                                Console.WriteLine("Digite a data de Nascimento para alterar");
                                novaData = DateTime.Parse(Console.ReadLine());
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("proc_EditaDataNasc", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                                sql_cmnd.Parameters.AddWithValue("@DataNasc", SqlDbType.Date).Value = novaData;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();

                                Console.WriteLine("Deseja alterar mais alguma informação do Cliente?:");
                                Console.WriteLine("\n1. Nome\n2. Data de Nascimento\n3. Sexo\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "3":
                                Console.WriteLine("Digite para qual sexo deseja alterar (M - Masculino ou F - Feminino)");
                                novoSexo = char.Parse(Console.ReadLine().ToUpper());
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("proc_EditaSexo", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                                sql_cmnd.Parameters.AddWithValue("@sexo", SqlDbType.Char).Value = novoSexo;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();


                                Console.WriteLine("Deseja alterar mais alguma informação do Cliente?:");
                                Console.WriteLine("\n1. Nome\n2. Data de Nascimento\n3. Sexo\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "0":
                                break;

                            default:
                                Console.WriteLine("Opção incorreta. Tente novamente.");
                                break;
                        }

                    } while (escolha != "0");
                }
                Console.WriteLine("Dados do cliente alterados com sucesso.\nPressione ENTER para voltar ao menu...");
                Console.ReadKey();
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }
        public static void LocalizaCliente()
        {
            var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            Console.WriteLine("Digite o nome OU o CPF do cliente que deseja buscar:");
            var Busca = Console.ReadLine();

            string Select = $"SELECT Nome, CPF, format(DataNasc,'dd/MM/yyyy'), sexo, Situacao FROM dbo.Cliente WHERE Nome like '%{Busca}%' OR CPF like '%{Busca}%'";
            using (SqlCommand comando = new SqlCommand(Select, cnn))
            {
                using (SqlDataReader Ler = comando.ExecuteReader())
                {
                    if (Ler.HasRows == true)
                    {
                        while (Ler.Read())
                        {
                            Console.Clear();
                            Console.WriteLine("Cliente encontrado: \n");
                            Console.WriteLine($"Nome: {Ler.GetString(0)}\nCPF: {Ler.GetString(1)}\nData de Nascimento: {Ler.GetString(2)}\nSexo:{Ler.GetString(3)}\nSituação: {Ler.GetString(4)}\n");
                            Console.WriteLine("Pressione ENTER para continuar...");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{Busca}\nCliente não encontrado. Pressione ENTER para voltar...");
                        Console.ReadKey();
                    }
                }
            }
            cnn.Close();
        }
        public static void LocalizaClienteBloqueado()
        {
            var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            Console.WriteLine("Digite o nome OU o CPF do cliente que deseja saber a situação:");
            var Busca = Console.ReadLine();

            string Select = $"SELECT Nome, CPF, Situacao FROM dbo.Cliente WHERE Nome like '%{Busca}%' OR CPF like '%{Busca}%'";
            using (SqlCommand comando = new SqlCommand(Select, cnn))
            {
                using (SqlDataReader Ler = comando.ExecuteReader())
                {
                    if (Ler.HasRows == true)
                    {
                        while (Ler.Read())
                        {
                            Console.Clear();
                            if (Ler.GetString(2) == "I")
                            {
                                Console.Clear();
                                Console.WriteLine($"O cliente {Ler.GetString(0)} encontra-se BLOQUEADO\nPressione ENTER para voltar...");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine($"O cliente {Ler.GetString(0)} encontra-se DESBLOQUEADO\nPressione ENTER para voltar...");
                                Console.ReadKey();
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{Busca}\nCliente não encontrado. Pressione ENTER para voltar...");
                        Console.ReadKey();
                    }
                }
            }
            cnn.Close();
        }



        public class Fornecedor
        {
            public string CNPJ { get; set; }
            public string RazaoSocial { get; set; }
            public DateTime DataAbertura { get; set; }
            public DateTime UltimaCompra { get; set; }
            public DateTime DataCadastro { get; set; }
            public char Situacao { get; set; }

            public Fornecedor()
            {

            }
            public Fornecedor(string cnpj, string rSocial, DateTime dAbertura, char situacao)
            {
                CNPJ = cnpj;
                RazaoSocial = rSocial;
                DataAbertura = dAbertura;
                UltimaCompra = DateTime.Now;
                DataCadastro = DateTime.Now;
                Situacao = situacao;
            }
            public Fornecedor(string cnpj, string rSocial, DateTime dAbertura, DateTime uCompra, DateTime dCadastro, char situacao) //construtor criado só pra ocupar espaço no codigo, pq nao usa pra nada.
            {
                CNPJ = cnpj;
                RazaoSocial = rSocial;
                DataAbertura = dAbertura;
                UltimaCompra = DateTime.Now;
                DataCadastro = DateTime.Now;
                Situacao = situacao;
            }
            public override string ToString()
            {
                return $"CNPJ: {CNPJ}\nRSocial: {RazaoSocial.Trim()}\nData de Abertura da empresa: {DataAbertura.ToString("dd/MM/yyyy")}\nUltima Compra: {UltimaCompra.ToString("dd/MM/yyyy")}\nData de Cadastro: {DataCadastro.ToString("dd/MM/yyyy")}\nSituacao: {Situacao}";
            }
        }
        public static void SubMenuFornecedores()
        {
            string escolha;

            do
            {
                Console.Clear();

                Console.WriteLine("=============== FORNECEDORES ===============");
                Console.WriteLine("1. Cadastar fornecedor");
                Console.WriteLine("2. Listar fornecedores");
                Console.WriteLine("3. Editar registro de fornecedor");
                Console.WriteLine("4. Bloquear/Desbloqueia fornecedor");
                Console.WriteLine("5. Localizar fornecedor");
                Console.WriteLine("6. Localizar fornecedor bloqueado");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("0. Voltar ao menu anterior");
                Console.Write("\nEscolha: ");

                switch (escolha = Console.ReadLine())
                {
                    case "0":
                        break;

                    case "1":
                        NovoFornecedor();
                        break;

                    case "2":
                        ListarFornecedores();
                        break;

                    case "3":
                        EditarFornecedor();
                        break;

                    case "4":
                        BloqueiaFornecedor();
                        break;

                    case "5":
                        LocalizaFornecedor();
                        break;

                    case "6":
                        LocalizaFornecedorBloqueado();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida");
                        Console.WriteLine("\n Pressione ENTER para voltar ao menu");
                        break;
                }

            } while (escolha != "0");
        }
        public static void NovoFornecedor()
        {
            Console.Clear();

            Console.Write("Digite a data de criacao da empresa:");
            DateTime dCriacao = DateTime.Parse(Console.ReadLine());
            DateTime DataAtual = DateTime.Now;
            TimeSpan date = DataAtual - dCriacao;
            var totalDias = date.Days;

            if (totalDias >= 180)
            {
                string rSocial, cnpj;
                char situacao;

                do
                {
                    Console.Write("CNPJ: ");
                    cnpj = Console.ReadLine();
                    //cnpj = cnpj.Trim();
                    cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                    if (Validacoes.ValidarCnpj(cnpj) == false)
                        Console.WriteLine("CNPJ Inválido. Digite um CNPJ Válido, ou 0 para sair.");
                } while (Validacoes.ValidarCnpj(cnpj) == false && cnpj != "0");
                if (cnpj == "0")
                    return;
                else
                {
                    Console.Write("Razao social: ");
                    rSocial = Console.ReadLine();
                    Console.Write("Situacao (A - Ativo/ I - Inativo): ");
                    situacao = char.Parse(Console.ReadLine().ToUpper());
                }
                try
                {
                    var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
                    SqlConnection cnn = new SqlConnection(connetionString);
                    using (cnn)
                    {

                        cnn.Open();
                        SqlCommand sql_cmnd = new SqlCommand("proc_AdicionaFornecedor", cnn);

                        sql_cmnd.CommandType = CommandType.StoredProcedure;

                        sql_cmnd.Parameters.AddWithValue("@CNPJ", SqlDbType.VarChar).Value = cnpj;
                        sql_cmnd.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = rSocial;
                        sql_cmnd.Parameters.AddWithValue("@DataAbertura", SqlDbType.Date).Value = dCriacao;
                        sql_cmnd.Parameters.AddWithValue("@Situacao", SqlDbType.Char).Value = situacao;
                        sql_cmnd.ExecuteNonQuery();
                        cnn.Close();
                        Console.WriteLine("Fornecedor incluido no banco de dados.");
                        Console.ReadKey();
                    }
                }
                catch (SqlException erro)
                {
                    Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                    Console.ReadKey();
                }
                return;

            }
            else
            {
                Console.WriteLine("Empresa com menos de 6 meses nao pode ser cadastrada. Pressione ENTER para voltar ao menu...");
                Console.ReadKey();
            }
        }
        public static void ListarFornecedores()
        {
            List<string> list = new List<string>();

            var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            string Select = $"Select RazaoSocial, CNPJ, Format(DataAbertura,'dd/MM/yyyy'), Situacao From dbo.Fornecedor";
            using (SqlCommand comando = new SqlCommand(Select, cnn))
            {
                using (SqlDataReader Ler = comando.ExecuteReader())
                {
                    if (Ler.HasRows == true)
                    {
                        while (Ler.Read())
                        {
                            list.Add($"Razão Social: {Ler.GetString(0)}\nCNPJ: {Ler.GetString(1)}\nData de Abertura: {Ler.GetString(2)}\nSituação: {Ler.GetString(3)}\n");
                        }
                        cnn.Close();
                        list.Sort();
                        int i = 0;
                        string opcao;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine(list[i]);
                            Console.WriteLine("\n\n1 - Proximo\t2 - Anterior\t3 - Primeiro\t4 - Ultimo\t0 - Sair");
                            opcao = Console.ReadLine();
                            switch (opcao)
                            {
                                case "1":
                                    if (i == list.Count() - 1)
                                        i = list.Count() - 1;
                                    else
                                        i++;
                                    break;

                                case "2":
                                    if (i == 0)
                                        i = 0;
                                    else
                                        i--;
                                    break;

                                case "3":
                                    i = 0;
                                    break;

                                case "4":
                                    i = list.Count() - 1;
                                    break;

                                case "0":
                                    break;

                                default:
                                    break;
                            }
                        }
                        while (opcao != "0");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Não existem Fornecedores cadastrados.");
                        Console.ReadKey();
                    }
                }
            }
        }
        public static void BloqueiaFornecedor()
        {

            Console.WriteLine("Insira o CNPJ para bloqueio: ");
            string cnpj = Console.ReadLine();
            cnpj = cnpj.Replace(".", "").Replace("-", "");
            Console.Write("Informe a situação do fornecedor (A - Ativo/ I - Inativo): ");
            char situacao = char.Parse(Console.ReadLine().ToUpper());
            try
            {
                var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
                SqlConnection cnn = new SqlConnection(connetionString);
                using (cnn)
                {

                    cnn.Open();
                    SqlCommand sql_cmnd = new SqlCommand("proc_SituacaoFornecedor", cnn);

                    sql_cmnd.CommandType = CommandType.StoredProcedure;

                    sql_cmnd.Parameters.AddWithValue("@CNPJ", SqlDbType.VarChar).Value = cnpj;
                    sql_cmnd.Parameters.AddWithValue("@Situacao", SqlDbType.Char).Value = situacao;
                    sql_cmnd.ExecuteNonQuery();
                    cnn.Close();
                    Console.WriteLine("Situação do fornecedor alterada para .'" + situacao + "'\nPressione ENTER para voltar ao menu...");
                    Console.ReadKey();


                }
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }




        }
        public static void EditarFornecedor()
        {
            Console.WriteLine("Informe o CNPJ do Fornecedor que deseja alterar:");
            var cnpj = Console.ReadLine().Replace(".", "").Replace("/", "").Replace("-", "");

            Console.Clear();
            Console.WriteLine("Informe qual dado deseja alterar do Fornecedor:");
            Console.WriteLine("1. Razão Social\n2. Data de Abertura");

            var escolha = Console.ReadLine();
            string novaRSocial;
            DateTime novaData;


            try
            {
                var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
                SqlConnection cnn = new SqlConnection(connetionString);
                SqlCommand sql_cmnd;
                using (cnn)
                {
                    do
                    {

                        switch (escolha)
                        {
                            case "1":
                                Console.WriteLine("Digite a Razão Social para alterar");
                                novaRSocial = Console.ReadLine();
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("proc_EditaRazaoSocial", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CNPJ", SqlDbType.VarChar).Value = cnpj;
                                sql_cmnd.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = novaRSocial;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();

                                Console.WriteLine("Deseja alterar mais alguma informação do Cliente?:");
                                Console.WriteLine("\n1. Razão Social\n2. Data de Abertura\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "2":
                                Console.WriteLine("Digite a Data de Abertura para alterar");
                                novaData = DateTime.Parse(Console.ReadLine());
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("proc_EditaDataAbertura", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CNPJ", SqlDbType.VarChar).Value = cnpj;
                                sql_cmnd.Parameters.AddWithValue("@DataAbertura", SqlDbType.Date).Value = novaData;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();

                                Console.WriteLine("Deseja alterar mais alguma informação do Fornecedor?:");
                                Console.WriteLine("\n1. Razão Social\n2. Data de Abertura\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "0":
                                break;

                            default:
                                Console.WriteLine("Opção incorreta. Tente novamente.");
                                break;
                        }

                    } while (escolha != "0");
                }
                Console.WriteLine("Dados do Fornecedor alterados com sucesso.\nPressione ENTER para voltar ao menu...");
                Console.ReadKey();
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }
        public static void LocalizaFornecedor()
        {
            var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            Console.WriteLine("Digite A Razão Social OU o CNPJ do cliente que deseja buscar:");
            var Busca = Console.ReadLine();

            string Select = $"Select RazaoSocial, CNPJ, Format(DataAbertura,'dd/MM/yyyy'), Situacao From dbo.Fornecedor WHERE RazaoSocial like '%{Busca}%' OR CNPJ like '%{Busca}%'";
            using (SqlCommand comando = new SqlCommand(Select, cnn))
            {
                using (SqlDataReader Ler = comando.ExecuteReader())
                {
                    if (Ler.HasRows == true)
                    {
                        while (Ler.Read())
                        {
                            Console.Clear();
                            Console.WriteLine("Empresa encontrado: \n");
                            Console.WriteLine($"Razão Social: {Ler.GetString(0)}\nCNPJ: {Ler.GetString(1)}\nData de Abertura: {Ler.GetString(2)}\nSituação: {Ler.GetString(3)}\n");
                            Console.WriteLine("Pressione ENTER para continuar...");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{Busca}\nRegistro não encontrado. Pressione ENTER para voltar...");
                        Console.ReadKey();
                    }
                }
            }
            cnn.Close();
        }
        public static void LocalizaFornecedorBloqueado()
        {
            var connetionString = @"Data Source=DESKTOP-9ETQ9P4;Initial Catalog=Biltiful;User ID=sa;Password=abc@123";
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();

            Console.WriteLine("Digite A Razão Social OU o CNPJ do cliente que deseja saber a situação:");
            var Busca = Console.ReadLine();

            string Select = $"Select RazaoSocial, CNPJ, Situacao From dbo.Fornecedor WHERE RazaoSocial like '%{Busca}%' OR CNPJ like '%{Busca}%'";
            using (SqlCommand comando = new SqlCommand(Select, cnn))
            {
                using (SqlDataReader Ler = comando.ExecuteReader())
                {
                    if (Ler.HasRows == true)
                    {
                        while (Ler.Read())
                        {
                            if (Ler.GetString(2) == "I")
                            {
                                Console.Clear();
                                Console.WriteLine($"Empresa {Ler.GetString(0)} encontra-se BLOQUEADA.\nPressione ENTER para voltar...");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine($"Empresa {Ler.GetString(0)} encontra-se DESBLOQUEADA.\nPressione ENTER para voltar...");
                                Console.ReadKey();
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{Busca}\nRegistro não encontrado. Pressione ENTER para voltar...");
                        Console.ReadKey();
                    }
                }
            }
            cnn.Close();
        }
    }

}    

        public class Validacoes
    {


        public static bool ValidarCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
        public static bool ValidarCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
}


