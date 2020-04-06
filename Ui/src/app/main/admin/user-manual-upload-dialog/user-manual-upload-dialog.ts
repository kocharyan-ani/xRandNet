import {Component, OnInit, ViewChild} from '@angular/core';
import {UploadService} from '../../services/upload-service/upload.service';
import {environment} from "../../../../environments/environment";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
    selector: 'user-manual-upload-dialog',
    templateUrl: './user-manual-upload-dialog.html',
    styleUrls: ['./user-manual-upload-dialog.css']
})
export class UserManualUploadDialog implements OnInit {

    @ViewChild('file', {static: false}) file;

    public userManualFile: File = null;

    constructor(public dialogRef: MatDialogRef<UserManualUploadDialog>, public uploadService: UploadService) {
    }

    ngOnInit() {
    }

    progress;
    canBeClosed = false;
    primaryButtonText = 'Upload';
    iconText = "cloud_upload";
    showCancelButton = true;
    uploading = false;
    uploadSuccessful = false;

    sizeHumanReadable(size) {
        size = size / (1024 * 1024);
        let sizeInString = String(size);
        sizeInString = sizeInString.substring(0, sizeInString.indexOf('.') + 2);
        return sizeInString
    }

    onFileAdded() {
        const files: { [key: string]: File } = this.file.nativeElement.files;
        for (let key in files) {
            if (!isNaN(parseInt(key))) {
                this.userManualFile = files[key];
            }
        }
        this.canBeClosed = true;
    }

    addFiles() {
        this.file.nativeElement.click();
    }

    upload() {
        if (this.uploadSuccessful) {
            return this.dialogRef.close();
        }
        this.uploading = true;
        this.progress = this.uploadService.upload(this.userManualFile, environment.apiUrl + '/api/files/UserManual');
        this.primaryButtonText = 'Finish';
        this.canBeClosed = false;
        this.dialogRef.disableClose = true;
        this.showCancelButton = false;
        this.progress.subscribe(() => {
            this.canBeClosed = true;
            this.dialogRef.disableClose = false;
            this.uploadSuccessful = true;
            this.iconText = 'done';
            this.uploading = false;
        });
    }
}
