import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { WishlistService } from '../../services/wishlist';
import { AuthService } from '../../services/auth';
import { Wishlist } from '../../models/wishlist.model';

interface DialogData {
  mode: 'create' | 'edit';
  wishlist?: Wishlist;
}

@Component({
  selector: 'app-wishlist-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './wishlist-form.html',
  styleUrls: ['./wishlist-form.css']
})
export class WishlistFormComponent {
  wishlistForm: FormGroup;
  loading = false;
  isEditMode: boolean;

  constructor(
    private fb: FormBuilder,
    private wishlistService: WishlistService,
    private authService: AuthService,
    private dialogRef: MatDialogRef<WishlistFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {
    this.isEditMode = data.mode === 'edit';
    
    this.wishlistForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.maxLength(500)]
    });

    if (this.isEditMode && data.wishlist) {
      this.wishlistForm.patchValue({
        name: data.wishlist.name,
        description: data.wishlist.description || ''
      });
    }
  }

  onSubmit(): void {
    if (this.wishlistForm.invalid) {
      return;
    }

    this.loading = true;

    if (this.isEditMode && this.data.wishlist) {
      const updateData = {
        id: this.data.wishlist.id,
        ...this.wishlistForm.value
      };
      this.wishlistService.update(updateData).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (err) => {
          console.error('Ошибка обновления:', err);
          this.loading = false;
        }
      });
    } else {
      this.wishlistService.create(this.wishlistForm.value).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (err) => {
          console.error('Ошибка создания:', err);
          this.loading = false;
        }
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}