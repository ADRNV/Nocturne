import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { UsersComponent } from '../../../features/administration/users/users.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';



@NgModule({
  declarations: [
    UsersComponent
  ],
  imports: [
    SharedModule,
    MatTableModule,
    MatPaginator,
    MatPaginatorModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
  ]
})
export class AdministrationModule { }
