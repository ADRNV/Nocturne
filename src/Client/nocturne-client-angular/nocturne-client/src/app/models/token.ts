export class Token {
    
    constructor(
        public accessToken: string,
        public refreshToken: RefreshToken){
    }
}
  
export class RefreshToken {
    public constructor(
    public id: string,
    public username: string,
    public tokenString: string,
    public expireAt: string){

    }
}