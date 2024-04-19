import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, lastValueFrom, map } from 'rxjs';
import { Livro } from '../../../../models/livro.model';

@Injectable({
  providedIn: 'root',
})
export class LivrosService {
  private readonly _url: string = 'http://localhost:5265/livro';

  constructor(private _http: HttpClient) {}

  getAll(): Observable<Livro[]> {
    return this._http.get<Livro[]>(this._url);
  }
}
