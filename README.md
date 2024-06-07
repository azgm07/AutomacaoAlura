# Decisões de Projeto
- Foi utilizado uma arquitetura aplicada a construção de um Console App em .Net para aplicar a Injeção de Dependencias.
- Foi separado a contrução do Console App das biblioteca de classes construídas em projetos diferentes dentro da mesma solução no Visual Studio, para ser possível aumentar o projeto para diferentes aplicações utilizando as mesmas classes.
- Foi utilizado um modelo MVC para a construção do App e adicionado dois serviços que são carregados por injeção de dependencias:
  - DataService: Responsável por tratar os dados, com a implementação para gravar os dados em arquivos Json.
  - WebDriverService: Responsável por chamar a controller do webdriver que cria e executa as funções.
- Foi organizado toda a parte de informação através do Logging da aplicação para ser mais extensível a outras aplicações.
- A execução da aplicação acontece em loop até que um sinal de desligamento seja executado, sendo adicionado técnicas de graceful shutdown que podem ser utilizados para tratar problemas de execução se necessários.
- Para a execução foi feito um método simples através da chamada do HostService que abriga a execução padrão, fazendo com que ele peça e espere uma entrada vinda do console para procurar e salvar o resultado.
