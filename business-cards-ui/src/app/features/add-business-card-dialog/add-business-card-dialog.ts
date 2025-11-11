import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, FormGroup } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../services/api.service';

export interface BusinessCardRequestDto {
  name: string;
  gender: string;
  dateOfBirth?: string | null;
  email: string;
  phone: string;
  address?: string | null;
  photo?: string | null;
}

@Component({
  selector: 'app-add-business-card-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule
  ],
  templateUrl: './add-business-card-dialog.html',
  styleUrls: ['./add-business-card-dialog.scss']
})
export class AddBusinessCardDialogComponent {
  submitting = false;
  photoBase64: string | null = null;
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private api: ApiService,
    private dialogRef: MatDialogRef<AddBusinessCardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      gender: ['Male', [Validators.required]],
      dateOfBirth: [''],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
      phone: ['', [Validators.required, Validators.maxLength(20)]],
      address: [''],
      photo: [null]
    });
  }

  async onFileChange(evt: Event) {
    const input = evt.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) {
      this.photoBase64 = null;
      return;
    }
    const file = input.files[0];
     if (file.size > 1_000_000) {
    alert('Photo must be ≤ 1MB');
    this.photoBase64 = null;
    (this.form.get('photo') as any)?.setValue(null);
    return;
  }
    this.photoBase64 = await this.fileToBase64(file);
  }

  private fileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onerror = () => reject(new Error('File read failed'));
      reader.onload = () => {
        const result = reader.result as string;
        const commaIdx = result.indexOf(',');
        resolve(commaIdx >= 0 ? result.substring(commaIdx + 1) : result);
      };
      reader.readAsDataURL(file);
    });
  }

  async submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting = true;

    const v = this.form.value as {
      name: string; gender: string; dateOfBirth?: string;
      email: string; phone: string; address?: string | null;
    };

    const dobIso = v.dateOfBirth ? `${v.dateOfBirth}T00:00:00` : undefined;

    const payload: BusinessCardRequestDto = {
      name: v.name.trim(),
      gender: v.gender,
      email: v.email.trim(),
      phone: v.phone.trim(),
      address: (v.address ?? '').trim() || null,
      dateOfBirth: dobIso,
      photo: this.photoBase64 ?? null
    };

    try {
      await this.api.addCard(payload);
      this.dialogRef.close(true);
    } catch (e) {
      console.error('Add card failed', e);
      this.submitting = false;
    }
  }

  cancel() {
    this.dialogRef.close(false);
  }
}
