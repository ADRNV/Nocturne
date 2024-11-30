export class RecordsResponse<T>{
    
    constructor(public records:T[], public totalCount: number){

    }
}