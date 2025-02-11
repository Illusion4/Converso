import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { JwtModule } from '@auth0/angular-jwt';
import { tokenGetter } from './core/services/auth.service';
import { httpAuthInterceptor } from './core/interceptors/http-auth.interceptor';
import { refreshTokenInterceptor } from './core/interceptors/refresh-token.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [importProvidersFrom(
    JwtModule.forRoot({
        config: {
            tokenGetter: tokenGetter,
        },
    }),
),provideRouter(routes), provideHttpClient(withInterceptors([httpAuthInterceptor, refreshTokenInterceptor])), provideAnimationsAsync()]
};
