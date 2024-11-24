import { Group } from "./group"

export interface User {
    id: string
    imageUrl: string
    userName: string
    login: string
    pasword: string
    role: string
    isOnline: boolean
    groups: Group[]
}
  