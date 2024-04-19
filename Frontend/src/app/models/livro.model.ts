export class Livro {
  id!: number;
  nome: string;
  numeroDePaginas: number;
  disponivel: boolean;
  dataPublicacao: Date;
  autorId: number;

  constructor() {
    this.nome = '';
    this.numeroDePaginas = 0;
    this.disponivel = true;
    this.dataPublicacao = new Date;
    this.autorId = 0;
  }
}
