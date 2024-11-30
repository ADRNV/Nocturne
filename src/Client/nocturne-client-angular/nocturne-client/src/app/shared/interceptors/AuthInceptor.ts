import { HttpEvent, HttpHandler, HttpHandlerFn, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";


export class AuthInterceptor implements HttpInterceptor{
    
    intercept(req: HttpRequest<unknown>,  handler: HttpHandler): Observable<HttpEvent<unknown>> {
        const token = localStorage.getItem('AuthToken');
    
        if(token){
            var withTokenRequest = req.clone({
                headers: req.headers.append('Authorization', token)
            });
    
            return handler.handle(withTokenRequest);
        } else {
            return handler.handle(req);
        }
    }
} 