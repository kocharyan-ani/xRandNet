import {Component, Inject} from '@angular/core';
import {Bug} from "../../../common/models/bug";
import {BugStatus} from "../../../common/models/bug-status";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../../environments/environment";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
    selector: 'bug-details-dialog',
    templateUrl: './bug-details-dialog.html',
    styleUrls: ['./bug-deatils-dialog.css']
})
export class BugDetailsDialog {


    constructor(public dialogRef: MatDialogRef<BugDetailsDialog>, @Inject(MAT_DIALOG_DATA) public bug: Bug,
                private httpClient: HttpClient) {

    }

    toBugStatusString(statusValue: number): string {
        if (statusValue == 0) {
            return "OPEN";
        } else if (statusValue == 1) {
            return "FIXED"
        } else if (statusValue == 2) {
            return "NOT A BUG"
        }
    }


    isBugClosed() {
        return this.bug.status !== 0;
    }

    onClickReopen() {
        this.bug.status = BugStatus.OPEN;
        this.httpClient.post(environment.apiUrl + "/api/app/bugs", this.bug.toJson()).toPromise()
    }

    onClickFixed() {
        this.bug.status = BugStatus.FIXED;
        this.httpClient.post(environment.apiUrl + "/api/app/bugs", this.bug.toJson()).toPromise()
    }

    onClickNotBug() {
        this.bug.status = BugStatus.NOT_A_BUG;
        this.httpClient.post(environment.apiUrl + "/api/app/bugs", this.bug.toJson()).toPromise()
    }
}
