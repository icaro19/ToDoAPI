import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn =
  (request, next) => {

    return next(request).pipe(
      catchError(error => {

        console.error(error);

        return throwError(() => error);
      })
    );
  };
