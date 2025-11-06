import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { productReducer } from './products/state/product.reducer';
import { ProductEffects } from './products/state/product.effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    provideStore({ products: productReducer }),
    provideEffects({ productEffects: ProductEffects }),
    importProvidersFrom(StoreDevtoolsModule.instrument({ maxAge: 25 })),
  ],
};
