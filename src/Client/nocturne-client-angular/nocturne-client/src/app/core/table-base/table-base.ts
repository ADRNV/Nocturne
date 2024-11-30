import { AfterViewInit, Directive, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";

@Directive({})
export abstract class TableBase<T> implements AfterViewInit{
    
    abstract ngAfterViewInit(): void;

    @ViewChild(MatPaginator) protected paginator!: MatPaginator;

    protected dataSource: MatTableDataSource<any> = new MatTableDataSource<T>([]);

    protected length: number = 0;

    displayedColumns:string[] = [];
}