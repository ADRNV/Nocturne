export interface Token {
    accessToken: string
    refreshToken: RefreshToken
}
  
export interface RefreshToken {
    id: string
    username: string
    tokenString: string
    expireAt: string
}