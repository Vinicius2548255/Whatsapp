class Contato
{
    string nome;
    string celular;
    List<Mensagem> mensagens;

    public Contato(string nome, string celular)
    {
        this.nome = nome;
        this.celular = celular;
    }

    override
    public string ToString()
    {
        return this.Celular + " - " + this.Nome;
    }

    internal List<Mensagem> Mensagens { get => mensagens; set => mensagens = value; }
    public string Nome { get => nome; set => nome = value; }
    public string Celular { get => celular; set => celular = value; }
}

class Mensagem
{
    Contato destinatario;
    string horaEnvio;
    string conteudo;
    MsgAudio msgAudio;
    MsgTexto msgTexto;
    MsgFoto msgFoto;

    public Mensagem(Contato destinatario, string horaEnvio, string conteudo, string tipo)
    {
        this.destinatario = destinatario;
        this.horaEnvio = horaEnvio;
        this.conteudo = conteudo;

        if (tipo == "audio")
        {
            this.msgAudio = new MsgAudio();
        }
        else if (tipo == "texto")
        {
            this.msgTexto = new MsgTexto();
        }
        else
        {
            this.msgFoto = new MsgFoto();
        }
    }

    internal MsgAudio MsgAudio { get => msgAudio; set => msgAudio = value; }
    internal MsgTexto MsgTexto { get => msgTexto; set => msgTexto = value; }
    internal MsgFoto MsgFoto { get => msgFoto; set => msgFoto = value; }

    override
    public string ToString()
    {
        string conteudo = this.conteudo;

        if (this.msgAudio != null)
        {
            conteudo = "Áudio enviado ";
        }
        else if (this.msgFoto != null)
        {
            conteudo = "Foto enviada";
        }

        return conteudo + " para " + this.destinatario.Nome + " as " + this.horaEnvio;
    }
}

class MsgTexto
{
    int numChar;
    public int NumChar { get => numChar; set => numChar = value; }
}

class MsgAudio
{
    int duracao;
    public int Duracao { get => duracao; set => duracao = value; }
}

class MsgFoto
{
    int tamanho;

    public int Tamanho { get => tamanho; set => tamanho = value; }
}

class WhatsApp
{
    List<Contato> contatinhos = new List<Contato>();
    List<Mensagem> mensagens = new List<Mensagem>();

    public List<Contato> listarContatinhos()
    {
        return this.contatinhos;
    }

    public List<Mensagem> listarMensagens()
    {
        return this.mensagens;
    }

    public void adicionarMensagem(Mensagem mensagem, string tipo)
    {
        this.mensagens.Add(mensagem);
    }

    public void adicionarContato(Contato contato)
    {
        this.contatinhos.Add(contato);
    }
}

class Program
{
    public static void ExibirLista<T>(List<T> lista, Boolean showIndex = false)
    {
        int i = 1;
        foreach (var item in lista)
        {
            if (showIndex)
            {
                Console.Write(i + " - " + item.ToString() + "\n");
            }
            else
            {
                Console.Write(item.ToString() + "\n");
            }

            i++;
        }

        //Console.Write("\n");
    }

    public static void ExibirMenu()
    {
        Console.Write("1 - Listar Mensagens\n");
        Console.Write("2 - Enviar Mensagem\n");
        Console.Write("3 - Listar Contatinhos\n");
        Console.Write("4 - Adicionar Contato\n");
        Console.WriteLine("Selecione a opção: ");
    }

    public static void EnviarMensagem(WhatsApp app, Contato destinario, string input, string tipo)
    {
        string horaAtual = DateTime.Now.ToString("HH:mm");

        Mensagem novaMensagem = new Mensagem(destinario, horaAtual, input, tipo);

        if (tipo == "audio")
        {
            novaMensagem.MsgAudio.Duracao = 12;
        }
        else if (tipo == "imagem")
        {
            novaMensagem.MsgFoto.Tamanho = 2300200;
        }
        else
        {
            novaMensagem.MsgTexto.NumChar = input.Length;
        }

        app.adicionarMensagem(novaMensagem, tipo);
    }

    public static void AdicionarContato(WhatsApp app, string inputNome, string inputTelefone)
    {
        app.adicionarContato((new Contato(inputNome, inputTelefone)));
    }

    public static Contato BuscarContato(List<Contato> lista, int index)
    {
        int i = 1;
        foreach (var contatinho in lista)
        {
            if (i == index)
            {
                return contatinho;
            }

            i++;
        }

        throw new Exception("Contato não encontrado.");
    }

    private static void ShowMenuMensagem()
    {
        Console.Write("\n");
        Console.Write("1 - Texto");
        Console.Write("\n");
        Console.Write("2 - Áudio");
        Console.Write("\n");
        Console.Write("3 - Foto");
        Console.Write("\n");
        Console.WriteLine("Selecione o tipo de mensagem: ");
    }

    private static void ProcessarInputTipoMensagem(WhatsApp app, Contato contatinhoSelecionado)
    {
        int opt;

        if (int.TryParse(Console.ReadLine(), out opt))
        {
            switch (opt)
            {
                case 1:
                    Console.WriteLine("Digite sua mensagem de texto: \n");
                    EnviarMensagem(app, contatinhoSelecionado, Console.ReadLine(), "texto");
                    break;

                case 2:
                    Console.WriteLine("Gravando...(aperte ENTER para enviar)");
                    Console.ReadLine();
                    Console.Write("Mensagem enviada com sucesso!\n");
                    EnviarMensagem(app, contatinhoSelecionado, null, "audio");
                    break;

                case 3:
                    Console.WriteLine("Selecione a imagem na galeria e aperte ENTER para enviar");
                    Console.ReadLine();
                    Console.Write("Foto enviada com sucesso!\n");
                    EnviarMensagem(app, contatinhoSelecionado, null, "imagem");
                    break;

                default:
                    throw new Exception("Opção inválida.");
                    break;
            }
        }
        else
        {
            throw new Exception("Caractér inválido.");
        }
    }

    public static void ProcessarOpcao(WhatsApp app)
    {
        int opt;

        if (int.TryParse(Console.ReadLine(), out opt))
        {
            switch (opt)
            {
                case 1:
                    if (app.listarMensagens().Count == 0)
                    {
                        Console.Write("Nenhuma mensagem enviada.\n");
                        break;
                    }

                    ExibirLista(app.listarMensagens());
                    break;

                case 2:
                    ExibirLista(app.listarContatinhos(), true);

                    if (app.listarContatinhos().Count == 0)
                    {
                        Console.Write("Nenhum contato encontrado.\n");
                        break;
                    }

                    Console.WriteLine("Selecione o contato: ");

                    int contatoSelecionado;

                    try
                    {
                        if (int.TryParse(Console.ReadLine(), out contatoSelecionado))
                        {
                            Contato contatinhoSelecionado = BuscarContato(app.listarContatinhos(), contatoSelecionado);

                            ShowMenuMensagem();

                            try
                            {
                                ProcessarInputTipoMensagem(app, contatinhoSelecionado);
                            }
                            catch (Exception exception)
                            {
                                Console.Write(exception.Message);
                            }
                        }
                        else
                        {
                            Console.Write("Informe um valor válido");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }

                    break;

                case 3:
                    if (app.listarContatinhos().Count == 0)
                    {
                        Console.Write("Nenhum contato encontrado.\n");
                        break;
                    }

                    ExibirLista(app.listarContatinhos(), true);

                    break;

                case 4:
                    Console.WriteLine("Informe o nome do contato: ");
                    string nome = Console.ReadLine();

                    Console.WriteLine("Informe o telefone: ");
                    string telefone = Console.ReadLine();

                    AdicionarContato(app, nome, telefone);
                    break;

                default:
                    Console.Write("Digite uma opção válida: ");
                    break;
            }
        }
        else
        {
            Console.Write("Digite uma opção válida: ");
        }
    }

    public static void Main()
    {
        WhatsApp app = new WhatsApp();

        while (true)
        {
            ExibirMenu();
            ProcessarOpcao(app);
        }
    }
}
