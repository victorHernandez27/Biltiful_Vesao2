using System;
using System.Data.SqlClient;

namespace Biltiful_Vesao2
{
    internal class Program
    {
        static void Main(string[] args)
        {





            string escolha;

            do
            {
                Console.Clear();

                Console.WriteLine("=============== MENU ===============");
                Console.WriteLine("1. Cadastro de Clientes");
                Console.WriteLine("2. Cadastro de Fornecedores");
                Console.WriteLine("3. Cadastro de Produtos");
                Console.WriteLine("4. Cadastro de Materias Prima");
                Console.WriteLine("5. Produção");
                Console.WriteLine("6. Compras");
                Console.WriteLine("7. Vendas");
                Console.WriteLine("------------------------------------");
                Console.WriteLine("0. Sair");
                Console.Write("\nEscolha: ");

                switch (escolha = Console.ReadLine())
                {
                    case "0":
                        Console.WriteLine("Programa Finalizado");
                        break;

                    case "1":
                        MenuCadastros.SubMenuClientes();
                        break;


                    case "2":
                        MenuCadastros.SubMenuFornecedores();
                        break;

                    case "3":
                        // new Produto().Menu();
                        break;

                    case "4":
                        //new MPrima().Menu();
                        break;

                    case "5":
                        // new Producao().SubMenu();
                        break;

                    case "6":
                        // Compra.SubMenu();
                        break;

                    case "7":
                        // MenuVendas.SubMenu();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida");
                        Console.WriteLine("\nPressione ENTER para voltar ao menu");
                        break;
                }

            } while (escolha != "0");
        }
    }
}