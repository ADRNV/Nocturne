import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { UsersComponent } from '../../../features/administration/users/users.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';



@NgModule({
  declarations: [
    UsersComponent
  ],
  imports: [
    SharedModule,
    MatTableModule,
    MatPaginator,
    MatPaginatorModule,
    MatTableModule
  ]
})
export class AdministrationModule { }
