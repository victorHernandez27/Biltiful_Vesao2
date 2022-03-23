using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Biltiful_Vesao2
//{
//    public class Venda
//    {
//        public static Arquivos caminho = new Arquivos();

//        public int Id { get; set; }
//        public string Cliente { get; set; }
//        public DateTime DataVenda { get; set; }
//        public decimal ValorTotal { get; set; }

//        public Venda()
//        {
//            Id = NovoIdVenda();
//            ValorTotal = 0;
//        }

//        public Venda(int id, string cliente, DateTime dataVenda, decimal vTotal)
//        {
//            Id = id;
//            Cliente = cliente;
//            DataVenda = dataVenda;
//            ValorTotal = vTotal;
//        }

//        public override string ToString()
//        {
//            return $"Venda Nº {Id.ToString().PadLeft(5, '0')}\tData: {DataVenda.ToString("dd/MM/yyyy")}\nCliente: {Cliente}\nTotal da Venda: {ValorTotal.ToString("00000.00").TrimStart('0')}";
//        }

//        public int NovoIdVenda()
//        {
//            try
//            {
//                return File.ReadAllLines(caminho.ArquivoVenda).Length + 1;
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Exception: " + e.Message);
//            }

//            return -1;
//        }

//        public void Cadastrar()
//        {
//            try
//            {
//                StreamWriter sw = new StreamWriter(caminho.ArquivoVenda, append: true);

//                sw.WriteLine(Id.ToString().PadLeft(5, '0') + Cliente.Replace(".", "").Replace("-", "") + DataVenda.ToString("dd/MM/yyyy").Replace("/", "") + ValorTotal.ToString("00000.00").Replace(",", ""));

//                sw.Close();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Exception: " + e.Message);
//            }
//        }

//        public Venda Localizar(int id)
//        {
//            string linha = LocalizarVendaPorId(id);

//            if (linha != null)
//            {
//                string idVenda = linha.Substring(0, 5);
//                string cliente = linha.Substring(5, 11);
//                string data = linha.Substring(16, 8);
//                string vtotal = linha.Substring(24, 7);

//                DateTime.TryParse(data.Insert(2, "/").Insert(5, "/"), out DateTime dt);

//                Venda venda = new Venda(int.Parse(idVenda), cliente.Insert(3, ".").Insert(7, ".").Insert(11, "-"), dt, Decimal.Parse(vtotal.Insert(vtotal.Length - 2, ",")));

//                return venda;
//            }

//            return null;
//        }

//        public string LocalizarVendaPorId(int id)
//        {
//            string[] dados = File.ReadAllLines(caminho.ArquivoVenda);

//            int minimo = 0;
//            int maximo = dados.Length;
//            int medio;

//            while (minimo <= maximo)
//            {
//                medio = (minimo + maximo) / 2;

//                if (id > medio)
//                    minimo = medio + 1;
//                else if (id < medio)
//                    maximo = medio - 1;
//                else
//                    return (medio != 0) ? dados[medio - 1] : null;
//            }

//            return null;
//        }

//        public void ImpressaoPorRegistro()
//        {
//            Console.Clear();

//            if (File.ReadAllLines(caminho.ArquivoItemVenda).Length == 0)
//            {
//                Console.Clear();
//                Console.WriteLine("Não ha vendas para exibir\nPressione ENTER para voltar...");
//                Console.ReadLine();
//                return;
//            }

//            string[] dados = File.ReadAllLines(caminho.ArquivoVenda);

//            var i = 0;
//            string choice;
//            ItemVenda itemVenda = new ItemVenda();

//            do
//            {
//                string idVenda = dados[i].Substring(0, 5);
//                string cpf = dados[i].Substring(5, 11);
//                string data = dados[i].Substring(16, 8);
//                string vtotal = dados[i].Substring(24, 7);

//                DateTime.TryParse(data.Insert(2, "/").Insert(5, "/"), out DateTime dt);

//                Venda venda = new Venda(int.Parse(idVenda), cpf.Insert(3, ".").Insert(7, ".").Insert(11, "-"), dt, Decimal.Parse(vtotal.Insert(vtotal.Length - 2, ",")));

//                Cliente cliente = new Read().ProcuraCliente(venda.Cliente);

//                List<ItemVenda> itens = itemVenda.Localizar(venda.Id);

//                Console.Clear();
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine("                           CLIENTE                        ");
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine($"Nome:\t\t{cliente.Nome.TrimStart(' ')}");
//                Console.WriteLine($"CPF:\t\t{cliente.CPF.Insert(3, ".").Insert(7, ".").Insert(11, "-")}");
//                Console.WriteLine($"Data Nasc.:\t{cliente.DataNascimento.ToString("dd/MM/yyyy")}");
//                Console.WriteLine($"Ultima Compra:\t{cliente.UltimaVenda.ToString("dd/MM/yyyy")}");
//                Console.WriteLine("\n\n----------------------------------------------------------");
//                Console.WriteLine($"Venda Nº {venda.Id.ToString().PadLeft(5, '0')}\t\t\tData: {venda.DataVenda.ToString("dd/MM/yyyy")}");
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine("Id\tProduto\t\tQtd\tV.Unitário\tT.Item");
//                Console.WriteLine("----------------------------------------------------------");
//                itens.ForEach(item => Console.WriteLine(item.ToString()));
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine($"\t\t\t\t\t\t{venda.ValorTotal.ToString("#.00")}");

//                Console.WriteLine("\n\n");
//                Console.WriteLine("1 - Proximo\t2 - Anterior\t3 - Primeiro\t4 - Ultimo\t0 - Cancelar");
//                choice = Console.ReadLine();
//                Console.Clear();
//                switch (choice)
//                {
//                    case "1":
//                        if (i == dados.Length - 1)
//                            i = dados.Length - 1;
//                        else
//                            i++;
//                        break;

//                    case "2":
//                        if (i == 0)
//                            i = 0;
//                        else
//                            i--;
//                        break;

//                    case "3":
//                        i = 0;
//                        break;

//                    case "4":
//                        i = dados.Length - 1;
//                        break;
//                    case "0":
//                        break;
//                    default:
//                        Console.WriteLine("Opção invalida. Tente novamente.");
//                        break;
//                }
//            } while (choice != "0");
//        }
//    }

//    public class Arquivos
//    {
//        public string CaminhoVenda { get; set; }
//        public string ArquivoVenda { get; set; }
//        public string ArquivoItemVenda { get; set; }

//        public Arquivos()
//        {
//            CaminhoVenda = Caminho();
//            ArquivoVenda = VerificaArquivo("Venda.dat");
//            ArquivoItemVenda = VerificaArquivo("ItemVenda.dat");
//        }

//        private string Caminho()
//        {
//            string caminho = Path.Combine(Directory.GetCurrentDirectory(), "DataBase");

//            if (!Directory.Exists(caminho))
//                Directory.CreateDirectory(caminho);

//            return caminho;
//        }

//        private string VerificaArquivo(string arquivo)
//        {
//            string arquivoDat = Path.Combine(CaminhoVenda, arquivo);

//            if (!File.Exists(arquivoDat))
//                File.Create(arquivoDat).Close();

//            return arquivoDat;
//        }




//    }

//    public class ItemVenda
//    {
//        private static Arquivos caminho = new Arquivos();

//        public int Id { get; set; }
//        public string Produto { get; set; }
//        public int Quantidade { get; set; }
//        public decimal ValorUnitario { get; set; }
//        public decimal TotalItem { get; set; }

//        public ItemVenda() { }

//        public ItemVenda(int id, string produto, int quantidade, decimal valorUnitario)
//        {
//            Id = id;
//            Produto = produto;
//            Quantidade = quantidade;
//            ValorUnitario = valorUnitario;
//            TotalItem = quantidade * valorUnitario;
//        }

//        public override string ToString()
//        {
//            return $"{Id.ToString().PadLeft(5, '0')}\t{Produto}\t{Quantidade.ToString().PadLeft(3, '0')}\t{ValorUnitario.ToString("000.00").TrimStart('0')}\t\t{TotalItem.ToString("0000.00").TrimStart('0')}";
//        }

//        public void Cadastrar(List<ItemVenda> itens)
//        {
//            try
//            {
//                StreamWriter sw = new StreamWriter(caminho.ArquivoItemVenda, append: true);

//                itens.ForEach(item =>
//                {
//                    string linha = item.Id.ToString().PadLeft(5, '0') + item.Produto + item.Quantidade.ToString().PadLeft(3, '0') + item.ValorUnitario.ToString("000.00").Replace(",", "") + item.TotalItem.ToString("0000.00").Replace(",", "");
//                    sw.WriteLine(linha);
//                });

//                sw.Close();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Exception: " + e.Message);
//            }
//        }

//        public List<ItemVenda> Localizar(int idVenda)
//        {
//            try
//            {
//                StreamReader sr = new StreamReader(caminho.ArquivoItemVenda);

//                List<ItemVenda> itens = new List<ItemVenda>();

//                string line;

//                while ((line = sr.ReadLine()) != null)
//                {
//                    int.TryParse(line.Substring(0, 5).TrimStart('0'), out int id);

//                    if (id == idVenda)
//                    {
//                        string produto = line.Substring(5, 13);
//                        string quantidade = line.Substring(18, 3);
//                        string valorUnitario = line.Substring(21, 5);

//                        Decimal.TryParse(valorUnitario.Insert(valorUnitario.Length - 2, ","), out decimal vUnitario);

//                        itens.Add(new ItemVenda(id, produto, int.Parse(quantidade), vUnitario));
//                    }
//                }

//                sr.Close();

//                return itens;
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Exception: " + e.Message);
//            }

//            return null;
//        }

//    }

//    public class MenuVendas
//    {
//        public static void SubMenu()
//        {
//            new Arquivos();

//            string opcao;

//            do
//            {
//                Console.Clear();

//                Console.WriteLine("=============== VENDAS ===============");
//                Console.WriteLine("1. Nova Venda");
//                Console.WriteLine("2. Consultar Venda");
//                Console.WriteLine("3. Imprimir Registros de Venda");
//                Console.WriteLine("--------------------------------------");
//                Console.WriteLine("0. Voltar");
//                Console.Write("\nEscolha: ");

//                switch (opcao = Console.ReadLine())
//                {
//                    case "1":
//                        NovaVenda();
//                        break;

//                    case "2":
//                        LocalizarVenda();
//                        break;
//                    case "3":
//                        new Venda().ImpressaoPorRegistro();
//                        break;
//                    case "0":
//                        break;

//                    default:
//                        Console.Clear();
//                        Console.WriteLine("Opção inválida");
//                        Console.WriteLine("\nPressione ENTER para voltar ao menu");
//                        break;
//                }
//            } while (opcao != "0");
//        }


//        public static void NovaVenda()
//        {
//            Console.Clear();

//            Cliente cliente;

//            Console.WriteLine("informe o CPF do cliente:");
//            string cpf = Console.ReadLine();

//            if (new Read().ProcurarCPFBloqueado(cpf) == true)
//            {
//                Console.Clear();
//                Console.WriteLine("\n Falha ao iniciar a venda. Procure pelo gerente do local.");
//                Console.WriteLine("\n Pressione ENTER para voltar ao menu");
//                Console.ReadKey();
//                return;
//            }
//            else
//            {
//                cliente = new Read().ProcuraCliente(cpf);

//                if (cliente == null)
//                {
//                    Console.Clear();
//                    Console.WriteLine("\nCliente não encontrado");
//                    Console.WriteLine("\n Pressione ENTER para voltar ao menu");
//                    Console.ReadKey();
//                    return;
//                }

//            }

//            Console.Clear();

//            Venda venda = new Venda();

//            venda.Cliente = cliente.CPF;
//            venda.DataVenda = DateTime.Now.Date;

//            Console.Write($"Venda Nº {venda.Id.ToString().PadLeft(5, '0')}\tData: {venda.DataVenda.ToString("dd/MM/yyyy")}");
//            Console.WriteLine();

//            List<ItemVenda> itensVenda = new List<ItemVenda>();

//            int itens = 1;
//            string escolha;

//            do
//            {
//                Produto produto;
//                int qtd = 0;
//                decimal totalItens = 0;

//                do
//                {
//                    produto = new Produto();

//                    Console.WriteLine("\nDigite o Código do Produto:");
//                    string codProduto = Console.ReadLine();

//                    produto = produto.RetornaProduto(codProduto);

//                    if (produto == null)
//                    {
//                        Console.WriteLine("\nProduto não encontrado ou código inválido.");
//                        Console.ReadKey();
//                        Console.Clear();
//                        continue;
//                    }
//                    else if (produto.Situacao.Equals('I'))
//                    {
//                        Console.WriteLine("\nProduto inativo ou código inválido.");
//                        Console.ReadKey();
//                        Console.Clear();
//                        continue;
//                    }

//                    Console.WriteLine("\nInforme a quantidade:");
//                    qtd = int.Parse(Console.ReadLine());


//                    if (qtd <= 0 || qtd > 999)
//                    {
//                        Console.WriteLine("Informe uma quantidade entre 1 e 999");
//                        Console.ReadKey();
//                        Console.Clear();
//                        continue;
//                    }

//                    totalItens = qtd * produto.ValorVenda;

//                    if (totalItens > (decimal)9999.99)
//                    {
//                        Console.WriteLine("Valor total dos item passou o limite permitido de $ 9.999,99");
//                        Console.ReadKey();
//                        Console.Clear();
//                        continue;
//                    }
//                } while ((qtd <= 0 || qtd > 999) || totalItens > (decimal)9999.99 || produto == null);

//                Console.Clear();


//                itensVenda.Add(new ItemVenda(venda.Id, produto.CodigoBarras, qtd, produto.ValorVenda));

//                Console.WriteLine("Id\tProduto\t\tQtd\tV.Unitário\tT.Item");
//                Console.WriteLine("------------------------------------------------------");

//                decimal valorTotal = 0;

//                itensVenda.ForEach(item =>
//                {
//                    Console.WriteLine(item.ToString());
//                    valorTotal += item.TotalItem;
//                    venda.ValorTotal = valorTotal;
//                });

//                Console.WriteLine("------------------------------------------------------");
//                Console.WriteLine($"\t\t\t\t\t\t{venda.ValorTotal.ToString("#.00")}");


//                do
//                {
//                    Console.WriteLine("\nAdicionar novo produto?");
//                    Console.WriteLine("[ S ] Sim\t[ N ] Não");
//                    escolha = Console.ReadLine().ToUpper();

//                    Console.Clear();
//                } while (escolha != "S" && escolha != "N");


//                if (escolha == "S")
//                    itens++;
//                else
//                    break;

//                if (itens == 4)
//                {
//                    Console.Clear();
//                    Console.WriteLine("Seu carrinho está cheio!");
//                    Console.ReadKey();
//                    break;
//                }

//            } while (itens != 4);


//            do
//            {
//                Console.Clear();
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine("                           CLIENTE                        ");
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine($"Nome:\t\t{cliente.Nome.TrimStart(' ')}");
//                Console.WriteLine($"CPF:\t\t{cliente.CPF.Insert(3, ".").Insert(7, ".").Insert(11, "-")}");
//                Console.WriteLine($"Data Nasc.:\t{cliente.DataNascimento.ToString("dd/MM/yyyy")}");
//                Console.WriteLine($"Ultima Compra:\t{cliente.UltimaVenda.ToString("dd/MM/yyyy")}");
//                Console.WriteLine("\n\n----------------------------------------------------------");
//                Console.WriteLine($"Venda Nº {venda.Id.ToString().PadLeft(5, '0')}\t\t\tData: {venda.DataVenda.ToString("dd/MM/yyyy")}");
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine("\n\nId\tProduto\t\tQtd\tV.Unitário\tT.Item");
//                Console.WriteLine("----------------------------------------------------------");
//                itensVenda.ForEach(item => Console.WriteLine(item.ToString()));
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine($"\t\t\t\t\t\t{venda.ValorTotal.ToString("#.00")}");

//                Console.WriteLine("\n\n");

//                Console.WriteLine("[ F ] Finalizar venda\t[ C ] Cancelar venda");
//                escolha = Console.ReadLine().ToUpper();

//            } while (escolha != "F" && escolha != "C");

//            if (escolha == "F")
//            {
//                ItemVenda itemVenda = new ItemVenda();

//                itensVenda.ForEach(item =>
//                {
//                    new Produto().Atualizar(item.Produto, venda.DataVenda.ToString("dd/MM/yyyy"));
//                });

//                itemVenda.Cadastrar(itensVenda);

//                venda.Cadastrar();

//                cliente.UltimaVenda = venda.DataVenda;

//                new Write().EditarCliente(cliente);

//                Console.WriteLine("\n\nVenda cadastrada com sucesso!\nPressione ENTER para voltar ao Menu Vendas...");

//                Console.ReadKey();
//            }
//        }

//        public static void LocalizarVenda()
//        {
//            Console.Clear();

//            Venda venda = new Venda();
//            ItemVenda itemVenda = new ItemVenda();

//            Console.WriteLine("Informe a venda que deseja buscar: ");
//            int.TryParse(Console.ReadLine(), out int id);
//            Console.WriteLine();

//            venda = venda.Localizar(id);

//            if (venda != null)
//            {
//                Cliente cliente = new Read().ProcuraCliente(venda.Cliente);
//                List<ItemVenda> itens = itemVenda.Localizar(venda.Id);

//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine("                           CLIENTE                        ");
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine($"Nome:\t\t{cliente.Nome.TrimStart(' ')}");
//                Console.WriteLine($"CPF:\t\t{cliente.CPF.Insert(3, ".").Insert(7, ".").Insert(11, "-")}");
//                Console.WriteLine($"Data Nasc.:\t{cliente.DataNascimento.ToString("dd/MM/yyyy")}");
//                Console.WriteLine($"Ultima Compra:\t{cliente.UltimaVenda.ToString("dd/MM/yyyy")}");
//                Console.WriteLine("\n\n----------------------------------------------------------");
//                Console.WriteLine($"Venda Nº {venda.Id.ToString().PadLeft(5, '0')}\t\t\tData: {venda.DataVenda.ToString("dd/MM/yyyy")}");
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine("Id\tProduto\t\tQtd\tV.Unitário\tT.Item");
//                Console.WriteLine("----------------------------------------------------------");
//                itens.ForEach(item => Console.WriteLine(item.ToString()));
//                Console.WriteLine("----------------------------------------------------------");
//                Console.WriteLine($"\t\t\t\t\t\t{venda.ValorTotal.ToString("#.00")}");

//                Console.WriteLine("\nPressione ENTER para voltar ao menu...\n");
//                Console.ReadLine();
//            }
//            else
//            {
//                Console.WriteLine("venda não registrada!\nPressione ENTER para voltar ao menu...");
//                Console.ReadLine();
//            }
//        }

//    }

//}