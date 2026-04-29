import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { WishService } from '../../services/wish';
import { Wish } from '../../models/wish.model';

interface DialogData {
  wishlistId: number;
  wish?: Wish;
  mode: 'create' | 'edit';
}

@Component({
  selector: 'app-wish-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './wish-form.html',
  styleUrls: ['./wish-form.css']
})
export class WishFormComponent {
  wishForm: FormGroup;
  loading = false;
  isEditMode: boolean;

  constructor(
    private fb: FormBuilder,
    private wishService: WishService,
    private dialogRef: MatDialogRef<WishFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {
    this.isEditMode = data.mode === 'edit';
    
    this.wishForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]], 
      description: [''],  
      link: [''],         
      price: [null]    
    });

    if (this.isEditMode && data.wish) {
      this.wishForm.patchValue({
        title: data.wish.title,
        description: data.wish.description || '',
        link: data.wish.link || '',
        price: data.wish.price || null
      });
    }
  }

  onSubmit(): void {
    if (this.wishForm.invalid) {
      return;
    }

    this.loading = true;
    
    const formValue = this.wishForm.value;
    const submitData = {
      title: formValue.title,
      description: formValue.description?.trim() === '' ? null : formValue.description,
      link: formValue.link?.trim() === '' ? null : formValue.link,
      price: formValue.price,
      wishlistId: this.data.wishlistId
    };

    if (this.isEditMode && this.data.wish) {
      this.wishService.update(this.data.wish.id, submitData).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (err) => {
          console.error('Ошибка обновления:', err);
          this.loading = false;
        }
      });
    } else {
      this.wishService.create(submitData).subscribe({
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