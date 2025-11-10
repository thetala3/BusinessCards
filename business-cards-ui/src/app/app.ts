import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ListBusinessCardsComponent } from './features/list-business-cards/list-business-cards';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ListBusinessCardsComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('business-cards-ui');
}
