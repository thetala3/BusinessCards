import {
  Component, OnInit, AfterViewInit, ViewChild, NgZone,
  ChangeDetectorRef, Inject, PLATFORM_ID
} from '@angular/core';
import { isPlatformBrowser, CommonModule, DatePipe } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { MatTableModule, MatTableDataSource, MatTable } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AddBusinessCardDialogComponent } from '../add-business-card-dialog/add-business-card-dialog';

export interface BusinessCard {
  id: string;
  name: string;
  gender: string;
  dateOfBirth: string | Date;
  email: string;
  phone: string;
  address: string;
  photo?: string | null;
}

@Component({
  selector: 'app-list-business-cards',
  standalone: true,
  imports: [
    CommonModule,
    DatePipe,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatToolbarModule,
    MatDividerModule,
    MatTooltipModule,
    MatDialogModule
  ],
  templateUrl: './list-business-cards.html',
  styleUrls: ['./list-business-cards.scss'],
})
export class ListBusinessCardsComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['photo','name','gender','dateOfBirth','email','phone','address','actions'];
  dataSource = new MatTableDataSource<BusinessCard>([]);
  loading = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<BusinessCard>;

  constructor(
    private apiService: ApiService,
    private ngZone: NgZone,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.dataSource.filterPredicate = (data: BusinessCard, filter: string) => {
      const f = (filter || '').trim().toLowerCase();
      return (
        (data.name ?? '').toLowerCase().includes(f) ||
        (data.email ?? '').toLowerCase().includes(f) ||
        (data.phone ?? '').toLowerCase().includes(f) ||
        (data.address ?? '').toLowerCase().includes(f)
      );
    };

    if (isPlatformBrowser(this.platformId)) {
      this.loadCards();
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  async loadCards(): Promise<void> {
    this.ngZone.run(() => { this.loading = true; this.cdr.detectChanges(); });
    try {
      debugger;
      const data = await this.apiService.getAllCards();
      this.ngZone.run(() => {
        this.dataSource.data = data ?? [];

        if (this.paginator && this.dataSource.paginator !== this.paginator) {
          this.dataSource.paginator = this.paginator;
        }
        if (this.sort && this.dataSource.sort !== this.sort) {
          this.dataSource.sort = this.sort;
        }
        (this.dataSource as any)._updateChangeSubscription?.();
        this.table?.renderRows();
        this.cdr.detectChanges();
      });
    } catch (error) {
      console.error('Error loading cards:', error);
    } finally {
      this.ngZone.run(() => { this.loading = false; this.cdr.detectChanges(); });
    }
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value ?? '';
    this.dataSource.filter = filterValue.trim().toLowerCase();
    this.dataSource.paginator?.firstPage();
  }

  async deleteCard(id: string): Promise<void> {
    await this.apiService.deleteCard(id);
    this.ngZone.run(() => {
      this.dataSource.data = this.dataSource.data.filter(c => c.id !== id);
      (this.dataSource as any)._updateChangeSubscription?.();
      this.table?.renderRows();
      this.cdr.detectChanges();
    });
  }


  openAddDialog() {
    const ref = this.dialog.open(AddBusinessCardDialogComponent, {
      width: '640px',
      disableClose: true,
    });
    ref.afterClosed().subscribe((saved: boolean) => {
      if (saved) this.loadCards();
    });
  }
}
