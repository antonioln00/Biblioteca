import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LivrosComponent } from './livros/livros.component';
const routes: Routes = [
 { path: 'livros', component: LivrosComponent }
];
@NgModule({
 imports: [RouterModule.forChild(routes)],
 exports: [RouterModule]
})
export class HomeRoutingModule { }
