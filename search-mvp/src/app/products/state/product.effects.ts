import { inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { ProductActions } from './product.actions';
import { ProductService } from '../product.service';
import { catchError, map, mergeMap, of } from 'rxjs';

export const ProductEffects = createEffect(
  () => {
    const actions$ = inject(Actions);
    const api = inject(ProductService);

    return actions$.pipe(
      ofType(ProductActions.loadAll, ProductActions.search, ProductActions.reindex),
      mergeMap((action) => {
        if (action.type === ProductActions.loadAll.type) {
          return api.all().pipe(
            map((products) => ProductActions.loadAllSuccess({ products })),
            catchError(() => of(ProductActions.loadAllSuccess({ products: [] })))
          );
        }
        if (action.type === ProductActions.search.type) {
          return api.search((action as any).q).pipe(
            map((results) => ProductActions.searchSuccess({ results })),
            catchError(() => of(ProductActions.searchSuccess({ results: [] })))
          );
        }
        return api.reindex().pipe(
          map(() => ProductActions.reindexSuccess()),
          catchError(() => of(ProductActions.reindexSuccess()))
        );
      })
    );
  },
  { functional: true }
);
