import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { UsersService } from '../../../shared/services/users.service';
import { map, merge, startWith, switchMap, take, tap } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { User } from '../../../models/user';
import { TableBase } from '../../../core/table-base/table-base';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent extends TableBase<User> {
  
  filterForm: FormGroup

  constructor(private usersService: UsersService){
    super();
    this.displayedColumns = ['id', 'userName', 'login', 'isOnline'];

    this.filterForm = new FormGroup(
      {
        id: new FormControl(),
        userName: new FormControl(),
        login: new FormControl() 
      }
    );
  }

  override ngAfterViewInit(): void {
    merge(this.paginator.page, this.filterForm.valueChanges)
    .pipe(
      startWith({}),
      switchMap(() => {
  
        let search: string[] = [];

        let id = this.filterForm.controls["id"].value;
        let userName = this.filterForm.controls['userName'].value;
        let login = this.filterForm.controls['login'].value;

        if(id) {search.push(`Id,Equals,${id}`)}
        if(userName){search.push(`UserName,Contains,${userName}`)}
        if(login){search.push(`UserName,Contains,${userName}`)}

        return this.usersService.getUsers(
          this.paginator.pageIndex,
          this.paginator.pageSize,
          search
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
