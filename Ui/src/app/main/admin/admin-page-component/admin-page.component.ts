import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service/authentication.service';
import {Title} from '@angular/platform-browser';
import {Bug} from "../../../common/models/bug";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {AboutInfo} from "../../../common/models/about-info";
import {environment} from "../../../../environments/environment";
import {Link, LinkType} from "../../../common/models/link";
import {SoftwareUploadDialog} from "../software-upload-dialog/software-upload-dialog";
import {UploadService} from "../../services/upload-service/upload.service";
import {UserManualUploadDialog} from "../user-manual-upload-dialog/user-manual-upload-dialog";
import {BugDetailsDialog} from "../bug-details-dialog/bug-details-dialog";
import {News} from "../../../common/models/news";
import {MatDialog} from "@angular/material/dialog";
import {JSONP_HOME} from "@angular/http/src/backends/browser_jsonp";

@Component({
    selector: 'app-admin-page',
    templateUrl: './admin-page.component.html',
    styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent implements OnInit {

    bugs: Array<Bug> = new Array<Bug>();
    aboutInfo: AboutInfo = new AboutInfo();
    news: Array<News> = new Array<News>();

    generalLinks: Array<Link> = Array<Link>();
    generalLinksBackup: Array<Link> = Array<Link>();

    literatureLinks: Array<Link> = Array<Link>();
    literatureLinksBackup: Array<Link> = Array<Link>();

    addGeneralLink: boolean = false;
    addLiteratureLink: boolean = false;

    newGeneralLink: Link = new Link();
    newLiteratureLink: Link = new Link();
    newPost: News = new News();

    softwareVersions: Array<string> = new Array<string>();
    selectedVersion: string = null;

    constructor(private loginService: AuthenticationService,
                private titleService: Title,
                private httpClient: HttpClient,
                public dialog: MatDialog,
                public uploadService: UploadService) {

    }

    editInfo() {
        this.httpClient.post(environment.apiUrl + '/api/data/info', this.aboutInfo.toJson())
            .subscribe((data) => {
                console.log(data);
            });
    }

    public openUserManualFileUploadPage() {
        let dialogRef = this.dialog.open(UserManualUploadDialog, {width: '500px', height: '300px'});
    }

    public openSoftwareUploadPage() {
        let dialogRef = this.dialog.open(SoftwareUploadDialog, {width: '50%', height: '50%'});
    }

    openBugDetailsPage(bug: Bug) {
        let dialogRef = this.dialog.open(BugDetailsDialog, {
            closeOnNavigation: false,
            hasBackdrop: true,
            autoFocus: true,
            disableClose: true,
            width: '600px',
            height: '700px',
            data: bug
        });

        dialogRef.afterClosed().subscribe((bug: Bug) => {
            console.log(bug)
        });
    }

    deleteBug(bug: Bug) {
        const options = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            body: bug.toJson()
        };
        this.httpClient.delete(environment.apiUrl + '/api/app/bugs', options).toPromise().then(() => {
            const index: number = this.bugs.indexOf(bug);
            if (index !== -1) {
                this.bugs.splice(index, 1);
            }
        })
    }

    editGeneralLink(link: Link) {
        this.generalLinksBackup.push(new Link(link.id, link.name, link.url, link.type))
    }

    editLiteratureLink(link: Link) {
        this.literatureLinksBackup.push(new Link(link.id, link.name, link.url, link.type))
    }

    deleteGeneralLink(link: Link) {
        const options = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            body: {},
            params: new HttpParams().append("id", link.id.toString())
        };
        this.httpClient.delete(environment.apiUrl + '/api/data/links', options).toPromise().then(() => {
            const index: number = this.generalLinks.indexOf(link);
            if (index !== -1) {
                this.generalLinks.splice(index, 1);
            }
            let indexOfDeletingLinkInEditingLinks: number = -1;
            this.generalLinksBackup.forEach((searchLink, i) => {
                if (link.id == searchLink.id) {
                    indexOfDeletingLinkInEditingLinks = i
                }
            });
            if (indexOfDeletingLinkInEditingLinks !== -1) {
                this.generalLinksBackup.splice(indexOfDeletingLinkInEditingLinks, 1);
            }
        })
    }

    deleteLiteratureLink(link: Link) {
        const options = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            body: {},
            params: new HttpParams().append("id", link.id.toString())
        };
        this.httpClient.delete(environment.apiUrl + '/api/data/links', options).toPromise().then(() => {
            const index: number = this.literatureLinks.indexOf(link);
            if (index !== -1) {
                this.literatureLinks.splice(index, 1);
            }
            let indexOfDeletingLinkInEditingLinks: number = -1;
            this.literatureLinksBackup.forEach((searchLink, i) => {
                if (link.id == searchLink.id) {
                    indexOfDeletingLinkInEditingLinks = i
                }
            });
            if (indexOfDeletingLinkInEditingLinks !== -1) {
                this.literatureLinksBackup.splice(indexOfDeletingLinkInEditingLinks, 1);
            }
        })
    }

    cancelEditingGeneralLink(link: Link) {
        let indexOfBackup: number = -1;
        this.generalLinksBackup.forEach((searchLink, i) => {
            if (link.id == searchLink.id) {
                indexOfBackup = i
            }
        });
        if (indexOfBackup !== -1) {
            link.name = this.generalLinksBackup[indexOfBackup].name;
            link.url = this.generalLinksBackup[indexOfBackup].url;
            this.generalLinksBackup.splice(indexOfBackup, 1);
        }
    }

    cancelEditingLiteratureLink(link: Link) {
        let indexOfBackup: number = -1;
        this.literatureLinksBackup.forEach((searchLink, i) => {
            if (link.id == searchLink.id) {
                indexOfBackup = i
            }
        });
        if (indexOfBackup !== -1) {
            link.name = this.literatureLinksBackup[indexOfBackup].name;
            link.url = this.literatureLinksBackup[indexOfBackup].url;
            this.literatureLinksBackup.splice(indexOfBackup, 1);
        }
    }

    saveGeneralLink(link: Link) {
        let indexOfSavedLinkInEditingLinks = -1;
        this.generalLinksBackup.forEach((searchLink, i) => {
            if (link.id == searchLink.id) {
                indexOfSavedLinkInEditingLinks = i
            }
        });
        if (indexOfSavedLinkInEditingLinks !== -1) {
            if (this.generalLinksBackup[indexOfSavedLinkInEditingLinks].url != link.url || this.generalLinksBackup[indexOfSavedLinkInEditingLinks].name != link.name) {
                this.httpClient.post(environment.apiUrl + "/api/data/links", link.toJson()).toPromise()
            }
            this.generalLinksBackup.splice(indexOfSavedLinkInEditingLinks, 1);
        }
    }

    saveLiteratureLink(link: Link) {
        let indexOfSavedLinkInEditingLinks = -1;
        this.literatureLinksBackup.forEach((searchLink, i) => {
            if (link.id == searchLink.id) {
                indexOfSavedLinkInEditingLinks = i
            }
        });
        if (indexOfSavedLinkInEditingLinks !== -1) {
            if (this.literatureLinksBackup[indexOfSavedLinkInEditingLinks].url != link.url || this.literatureLinksBackup[indexOfSavedLinkInEditingLinks].name != link.name) {
                this.httpClient.post(environment.apiUrl + "/api/data/links", link.toJson()).toPromise()
            }
            this.literatureLinksBackup.splice(indexOfSavedLinkInEditingLinks, 1);
        }
    }

    addNewGeneralLink(newLink: Link) {
        newLink.type = LinkType.GENERAL;
        console.log(newLink.toJson());
        this.httpClient.put(environment.apiUrl + "/api/data/links", newLink.toJson()).toPromise().then((newLink: Link) => {
            this.generalLinks.push(new Link(newLink.id, newLink.name, newLink.url, LinkType.GENERAL));
        });
        this.newGeneralLink = new Link()
    }

    addNewLiteratureLink(newLink: Link) {
        newLink.type = LinkType.LITERATURE;
        console.log(newLink.toJson());
        this.httpClient.put(environment.apiUrl + "/api/data/links", newLink.toJson()).toPromise().then((newLink: Link) => {
            this.literatureLinks.push(new Link(newLink.id, newLink.name, newLink.url, LinkType.LITERATURE));
        });
        this.newLiteratureLink = new Link()
    }

    onVersionChanged() {
        this.bugs = [];
        let params = new HttpParams().set('version', this.selectedVersion);
        this.httpClient.get(environment.apiUrl + '/api/app/bugs', {params})
            .subscribe((data: Array<Bug>) => {
                Array.from(data).forEach((bugObj) => {
                    this.bugs.push(new Bug(bugObj.id, bugObj.summary, bugObj.description, bugObj.reporter, bugObj.version, bugObj.status, bugObj.reportDate))
                });
            });
    }

    ngOnInit(): void {
        this.titleService.setTitle('Admin');
        this.httpClient.get(environment.apiUrl + '/api/data/news')
            .subscribe((data: Array<News>) => {
                data.forEach((news) => {
                    let post = new News(news.title, news.datePosted, news.content, news.id);
                    post.editable = false;
                    this.news.push(post)
                })
            });
        this.httpClient.get(environment.apiUrl + '/api/app/versions')
            .subscribe((data: Array<object>) => {
                for (let obj of data) {
                    this.softwareVersions.push(obj["version"]);
                }
                this.softwareVersions.sort((a, b) =>
                    a.localeCompare(b)
                ).reverse();
                this.selectedVersion = this.softwareVersions[0];
                this.onVersionChanged();
            });
        this.httpClient.get(environment.apiUrl + '/api/data/info')
            .subscribe((aboutInfo: AboutInfo) => {
                if (aboutInfo != null) {
                    this.aboutInfo = new AboutInfo(aboutInfo.id, aboutInfo.content);
                } else {
                    this.aboutInfo = new AboutInfo()
                }
            });
        let params = new HttpParams().set('type', LinkType.GENERAL.toString());
        this.httpClient.get(environment.apiUrl + '/api/data/links', {params})
            .subscribe((links: Array<Link>) => {
                if (links != null && links.length != 0) {
                    links.forEach(link => {
                        this.generalLinks.push(new Link(link.id, link.name, link.url, link.type))
                    });
                }
            });
        params = new HttpParams().set('type', LinkType.LITERATURE.toString());
        this.httpClient.get(environment.apiUrl + '/api/data/links', {params})
            .subscribe((links: Array<Link>) => {
                if (links != null && links.length != 0) {
                    links.forEach(link => {
                        this.literatureLinks.push(new Link(link.id, link.name, link.url, link.type))
                    });
                }
            });
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

    editNews(n) {
        n.editable = true
    }

    saveNews(n) {
        this.httpClient.post(environment.apiUrl + '/api/data/news', JSON.parse(JSON.stringify(n))).toPromise().then(() => {
            n.editable = false
        })
    }

    addNews(n) {
        this.httpClient.put(environment.apiUrl + "/api/data/news", n.toJson()).toPromise().then((newPost: News) => {
            this.news.push(new News(newPost.title, newPost.datePosted, newPost.content, newPost.id));
            this.newPost = new News()
        });
    }


    deleteNews(news: News) {
        const options = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            body: news.toJson()
        };
        this.httpClient.delete(environment.apiUrl + '/api/data/news', options).toPromise().then(() => {
            const index: number = this.news.indexOf(news);
            if (index !== -1) {
                this.news.splice(index, 1);
            }
        })
    }
}
