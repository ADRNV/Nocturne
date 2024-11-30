import { HttpEvent, HttpHandler, HttpHandlerFn, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";


export class AuthInterceptor implements HttpInterceptor{
    
    intercept(req: HttpRequest<unknown>,  handler: HttpHandler): Observable<HttpEvent<unknown>> {
        
        const tokenRaw = localStorage.getItem('AuthToken');

        if(tokenRaw){

            let token = JSON.parse(tokenRaw);
            
            var withTokenRequest = req.clone({
                headers: req.headers.append('Authorization', "Bearer " + token.accessToken)
            });
    
            return handler.handle(withTokenRequest);
        } else {
            return handler.handle(req);
        }
    }
} 