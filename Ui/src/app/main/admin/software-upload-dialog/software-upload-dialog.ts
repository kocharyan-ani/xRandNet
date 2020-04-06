import {Component, OnInit, ViewChild} from '@angular/core';
import {environment} from "../../../../environments/environment";
import {Software} from "../../../common/models/software";
import {HttpClient, HttpEventType, HttpRequest, HttpResponse} from "@angular/common/http";
import {Subject} from "rxjs";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
    selector: 'software-upload-dialog',
    templateUrl: './software-upload-dialog.html',
    styleUrls: ['./software-upload-dialog.css']
})
export class SoftwareUploadDialog implements OnInit {

    @ViewChild('file', {static: false}) file;

    public softwareFile: File = null;
    public software: Software = new Software();
    releaseNotes: String = '';

    constructor(public dialogRef: MatDialogRef<SoftwareUploadDialog>, private http: HttpClient) {
    }

    ngOnInit() {
    }

    sizeHumanReadable(size) {
        size = size / (1024 * 1024);
        let sizeInString = String(size);
        sizeInString = sizeInString.substring(0, sizeInString.indexOf('.') + 2);
        return sizeInString
    }

    progress;
    canBeClosed = false;
    primaryButtonText = 'Upload';
    iconText = "cloud_upload";
    showCancelButton = true;
    uploading = false;
    uploadSuccessful = false;

    onFileAdded() {
        const files: { [key: string]: File } = this.file.nativeElement.files;
        for (let key in files) {
            if (!isNaN(parseInt(key))) {
                this.softwareFile = files[key];
            }
        }
        this.canBeClosed = true
    }

    addFiles() {
        this.file.nativeElement.click();
    }

    validateAndStoreReleaseNotes(): boolean {
        let releaseNoteDescriptions: Array<String> = this.releaseNotes.split('|');
        this.software.releaseNotes = releaseNoteDescriptions.join(" | ");
        return true;
    }

    upload() {
        if (this.uploadSuccessful) {
            return this.dialogRef.close();
        }
        if (!this.validateAndStoreReleaseNotes())
            return;
        const formData: FormData = new FormData();
        formData.append('file', this.softwareFile, this.softwareFile.name);
        formData.append('software', this.software.toString());
        let req = new HttpRequest('PUT', environment.apiUrl + '/api/app', formData, {
            reportProgress: true
        });
        const progress = new Subject<number>();
        this.http.request(req).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
                const percentDone = Math.round((100 * event.loaded) / event.total);
                progress.next(percentDone);
            } else if (event instanceof HttpResponse) {
                progress.complete();
            }
        });
        this.progress = progress.asObservable();
        this.uploading = true;
        this.primaryButtonText = 'Finish';
        this.iconText = "done";
        this.canBeClosed = false;
        this.dialogRef.disableClose = true;
        this.showCancelButton = false;
        this.progress.subscribe(() => {
            this.canBeClosed = true;
            this.dialogRef.disableClose = false;
            this.uploadSuccessful = true;
            this.uploading = false;
        });
    }

}
