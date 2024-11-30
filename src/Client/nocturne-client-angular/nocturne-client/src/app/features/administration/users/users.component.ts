import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { UsersService } from '../../../shared/services/users.service';
import {MatTableModule} from '@angular/material/table';
import { map, merge, startWith, switchMap, take, tap } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { User } from '../../../models/user';
import { EventEmitter } from 'stream';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements AfterViewInit {
  
  dataSource: MatTableDataSource<User> = new MatTableDataSource<User>([]);

  length: number = 0;

  displayedColumns = ['id', 'userName', 'login', 'isOnline'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private usersService: UsersService){

  }

  ngAfterViewInit(): void {
    this.paginator.length = 100;
    merge(this.paginator.page)
    .pipe(
      startWith({}),
      switchMap(() => {
        
        return this.usersService.getUsers(
          this.paginator.pageIndex+1,
          this.paginator.pageSize
        );
      }),
      map(data => {
        this.length = data.totalCount;
        this.dataSource.data = data.records;
        return data.records;
      })
    ).subscribe(data => {
      this.dataSource.data = data;
    });
  }

  onPage(e: any){
    console.log(e);
  }

}
