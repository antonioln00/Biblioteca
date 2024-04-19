import { Component } from '@angular/core';
import { LivrosService } from './livros.service';

@Component({
  selector: 'app-livros',
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.css',
})
export class LivrosComponent {
  constructor(private _livrosService: LivrosService) {}

  listarLivros(){
    this._livrosService.getAll().subscribe({
      next: jsonLivros => {
        console.log(jsonLivros);
      }
    })
  }
}
