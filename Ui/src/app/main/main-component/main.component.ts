import {MediaMatcher} from '@angular/cdk/layout';
import {ChangeDetectorRef, Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Title} from '@angular/platform-browser';
import {Subscription} from 'rxjs';
import {User} from 'src/app/common/models/user';
import {AuthenticationService} from '../services/authentication-service/authentication.service';
import {Bug} from "../../common/models/bug";
import {HttpOptionsService} from "../../common/http/services/http-options.service";
import {BugStatus} from "../../common/models/bug-status";
import {environment} from "../../../environments/environment";
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";

@Component({
    selector: 'app-main',
    templateUrl: './main.component.html',
    styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit, OnDestroy {
    mobileQuery: MediaQueryList;
    userManualFileUrl: string = environment.apiUrl + "/api/files/userManual";
    currentUser: User;
    currentUserSubscription: Subscription;

    private _mobileQueryListener: () => void;

    constructor(changeDetectorRef: ChangeDetectorRef, private httpClient: HttpClient,
                private httpOptionsService: HttpOptionsService,
                private titleService: Title,
                private authenticationService: AuthenticationService,
                public dialog: MatDialog, media: MediaMatcher) {
        this.currentUserSubscription = this.authenticationService.currentUser.subscribe(user => {
            this.currentUser = user;
            this.mobileQuery = media.matchMedia('(max-width: 992px)');
            this._mobileQueryListener = () => changeDetectorRef.detectChanges();
            this.mobileQuery.addListener(this._mobileQueryListener);
        });


    }

    ngOnInit() {
        this.titleService.setTitle('xRandNet');
    }


    openDialog() {

        const dialogRef = this.dialog.open(BugReportDialog, {
            closeOnNavigation: false,
            hasBackdrop: true,
            autoFocus: true,
            disableClose: true,
            width: '400px',
            height: '600px',
            data: {
                'bug': new Bug(null, null, null, null, null, null, null),
                'currentUser': this.currentUser
            }
        });
    }


    ngOnDestroy() {
        // unsubscribe to ensure no memory leaks
        this.currentUserSubscription.unsubscribe();
        this.mobileQuery.removeListener(this._mobileQueryListener);
    }

    logout() {
        this.authenticationService.logout();
        this.mobileQuery.removeListener(this._mobileQueryListener);
    }
}

@Component({
    selector: 'bug-report-dialog',
    templateUrl: 'bug.report.dialog.html',
    styleUrls: ['bug.report.dialog.css']
})
export class BugReportDialog implements OnInit {

    softwareVersions: Array<string> = new Array<string>();
    bug: Bug;
    currentUser: User;
    successfullySent = false;

    constructor(
        private httpClient: HttpClient,
        public dialogRef: MatDialogRef<BugReportDialog>,
        @Inject(MAT_DIALOG_DATA) public data: Object) {
        this.bug = data['bug'];
        this.bug.status = BugStatus.OPEN;
        this.currentUser = data['currentUser'];
    }

    onSendClick() {
        if (this.bug && this.currentUser) {
            this.bug.id = 0;
            this.bug.reporter = this.currentUser.username;
        }
        if (this.bug && this.bug.description && this.bug.summary && this.bug.version) {
            this.httpClient.put(environment.apiUrl + "/api/app/bugs", this.bug.toJson()).subscribe((bug: Bug) => {
                this.bug = bug;
                this.successfullySent = true
            })
        }
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    isDialogValid() {
        return this.bug && this.bug.summary && this.bug.description && this.bug.version;
    }

    ngOnInit(): void {
        this.httpClient.get(environment.apiUrl + "/api/app/versions").subscribe((versions: Array<object>) => {
            if (versions != null) {
                this.softwareVersions = [];
                for (let obj of versions) {
                    this.softwareVersions.push(obj["version"]);
                }
                this.softwareVersions.sort((a, b) =>
                    a.localeCompare(b)
                ).reverse();
            }
        })

    }

}
