import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { merge } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  public usuario: string = '';
  public senha: string | number = '';
  hide = true;
  constructor(private router: Router) {}

  ngOnInit() {}

  public realizarLogin() {
    this.router.navigate(['/home']);
  }
}
