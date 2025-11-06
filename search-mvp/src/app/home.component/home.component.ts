import { Component, inject, signal } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectAll, selectLoading, selectResults } from '../products/state/product.selectors';
import { ProductActions } from '../products/state/product.actions';
import { CommonModule, CurrencyPipe } from '@angular/common';
@Component({
  selector: 'app-home',
  imports: [CommonModule, CurrencyPipe],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  private store = inject(Store);
  q = signal('');

  all$ = this.store.select(selectAll);
  results$ = this.store.select(selectResults);
  loading$ = this.store.select(selectLoading);

  ngOnInit() {
    this.store.dispatch(ProductActions.loadAll());
  }

  onSearch() {
    this.onReindex();
    this.store.dispatch(ProductActions.search({ q: this.q() }));
  }

  onReindex() {
    this.store.dispatch(ProductActions.reindex());
  }
}
