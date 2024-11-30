import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { UsersService } from '../../../shared/services/users.service';
import { map, merge, startWith, switchMap, take, tap } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { User } from '../../../models/user';
import { TableBase } from '../../../core/table-base/table-base';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent extends TableBase<User> {
  
  constructor(private usersService: UsersService){
    super();
    this.displayedColumns = ['id', 'userName', 'login', 'isOnline'];
  }

  override ngAfterViewInit(): void {
    merge(this.paginator.page)
    .pipe(
      startWith({}),
      switchMap(() => {
  
        return this.usersService.getUsers(
          this.paginator.pageIndex,
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
}
