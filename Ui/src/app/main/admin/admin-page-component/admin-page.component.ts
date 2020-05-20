import {Component, OnInit, ViewChild} from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service/authentication.service';
import {Title} from '@angular/platform-browser';
import {Bug} from "../../../common/models/bug";
import {HttpClient, HttpEventType, HttpHeaders, HttpParams, HttpRequest, HttpResponse} from "@angular/common/http";
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
import {Person} from "../../../common/models/person";
import {Publication} from "../../../common/models/publication";
import {Project} from "../../../common/models/project";
import {Subject} from "rxjs";

@Component({
    selector: 'app-admin-page',
    templateUrl: './admin-page.component.html',
    styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent implements OnInit {

    @ViewChild('file', {static: false}) file;

    showHomeSection: boolean = true;
    showSoftwareSection: boolean = false;
    showLinksSection: boolean = false;
    showPeopleSection: boolean = false;
    showNewsSection: boolean = false;
    showPublicationsSection: boolean = false;
    showProjectsSection: boolean = false;

    bugs: Array<Bug> = new Array<Bug>();
    aboutInfo: AboutInfo = new AboutInfo();
    news: Array<News> = new Array<News>();
    people: Array<Person> = new Array<Person>();
    projects: Array<Project> = new Array<Project>();
    publications: Array<Publication> = new Array<Publication>();

    newGeneralLink: Link = new Link();
    newLiteratureLink: Link = new Link();
    newPost: News = new News();
    newPerson: Person = new Person();
    newPublication: Publication = new Publication();
    newProject: Project = new Project();
    generalLinks: Array<Link> = Array<Link>();
    generalLinksBackup: Array<Link> = Array<Link>();

    literatureLinks: Array<Link> = Array<Link>();
    literatureLinksBackup: Array<Link> = Array<Link>();

    addGeneralLink: boolean = false;
    addLiteratureLink: boolean = false;

    softwareVersions: Array<string> = new Array<string>();
    selectedVersion: string = null;

    constructor(private loginService: AuthenticationService,
                private titleService: Title,
                private httpClient: HttpClient,
                public dialog: MatDialog,
                public uploadService: UploadService) {

    }

    ngOnInit(): void {
        this.titleService.setTitle('Admin');
        this.httpClient.get(environment.apiUrl + '/api/data/news')
            .subscribe((data: Array<News>) => {
                data.sort((a, b) => {
                    return b.id - a.id
                })
                data.forEach((news) => {
                    let post = new News(news.title, news.datePosted, news.content, news.id);
                    post.editable = false;
                    this.news.push(post)
                })
            });
        this.httpClient.get(environment.apiUrl + '/api/data/people')
            .subscribe((data: Array<Person>) => {
                data.forEach((p) => {
                    let person = new Person(p.id, p.firstName, p.lastName, p.facebookUrl,
                        p.imageUrl, p.email, p.linkedInUrl, p.description);
                    person.editable = false;
                    this.people.push(person)
                })
            });
        this.httpClient.get(environment.apiUrl + '/api/data/publications')
            .subscribe((data: Array<Publication>) => {
                data.forEach((p) => {
                    let publication = new Publication(p.id, p.title, p.authors, p.journal)
                    publication.editable = false;
                    this.publications.push(publication)
                })
            });
        this.httpClient.get(environment.apiUrl + '/api/data/projects')
            .subscribe((data: Array<Project>) => {
                data.forEach((p) => {
                    let project = new Project(p.id, p.name, p.description);
                    project.editable = false;
                    this.projects.push(project)
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
            width: '800px',
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
            this.news.unshift(new News(newPost.title, newPost.datePosted, newPost.content, newPost.id));
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

    editPerson(person) {
        person.editable = true
    }

    savePerson(person) {
        this.httpClient.post(environment.apiUrl + '/api/data/people', person.toJson()).toPromise().then(() => {
            person.editable = false
        })
    }

    deletePerson(person) {
        const options = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            body: person.toJson()
        };
        this.httpClient.delete(environment.apiUrl + '/api/data/people', options).toPromise().then(() => {
            const index: number = this.people.indexOf(person);
            if (index !== -1) {
                this.people.splice(index, 1);
            }
        })
    }

    addPerson(person) {
        this.httpClient.put(environment.apiUrl + "/api/data/people", person.toJson()).toPromise().then((newPerson: Person) => {
            this.people.push(new Person(newPerson.id, newPerson.firstName, newPerson.lastName,
                newPerson.facebookUrl, newPerson.imageUrl, newPerson.email, newPerson.linkedInUrl,
                newPerson.description));
            this.newPerson = new Person()
        });
    }

    getImageUrl(person) {
        if (person.imageUrl == null || person.imageUrl == '') {
            return 'https://shorturl.at/kqRS6'
        }
        return person.imageUrl
    }

    showHome() {
        this.reset()
        this.showHomeSection = true;
    }

    showSoftware() {
        this.reset()
        this.showSoftwareSection = true;
    }

    showNews() {
        this.reset()
        this.showNewsSection = true;
    }

    showPeople() {
        this.reset()
        this.showPeopleSection = true;
    }

    showLinks() {
        this.reset()
        this.showLinksSection = true;
    }

    showPublications() {
        this.reset()
        this.showPublicationsSection = true;
    }

    showProjects() {
        this.reset()
        this.showProjectsSection = true;
    }

    reset() {
        this.showHomeSection = false;
        this.showLinksSection = false;
        this.showNewsSection = false;
        this.showPeopleSection = false;
        this.showSoftwareSection = false;
        this.showProjectsSection = false;
        this.showPublicationsSection = false;
    }

    addPublicationFile() {
        this.file.nativeElement.click();
    }

    onPublicationFileAdded(publication) {
        const files: { [key: string]: File } = this.file.nativeElement.files;
        for (let key in files) {
            if (!isNaN(parseInt(key))) {
                publication.file = files[key];
            }
        }
    }

    addPublication(publication) {
        const formData: FormData = new FormData();
        formData.append('file', publication.file, publication.file.name);
        formData.append('publication', JSON.stringify(publication.toJson()));
        let req = new HttpRequest('PUT', environment.apiUrl + '/api/data/publications', formData, {
            reportProgress: true
        });
        this.httpClient.request(req).subscribe((event) => {
            let publication: Publication = event['body']
            if (publication !== undefined)
                this.publications.push(new Publication(publication.id, publication.title, publication.authors,
                    publication.journal));
            this.newPublication = new Publication()
        });
    }

    downloadPublication(publication) {
        window.location.href = environment.apiUrl + "/api/data/publications?publicationId=" + publication.id;
    }

    editPublication(publication) {
        publication.editable = true
    }

    savePublication(publication) {
        this.httpClient.post(environment.apiUrl + '/api/data/publications', publication.toJson()).toPromise().then(() => {
            publication.editable = false
        })
    }

    deletePublication(publication) {
        const options = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            body: publication.toJson()
        };
        this.httpClient.delete(environment.apiUrl + '/api/data/publications', options).toPromise().then(() => {
            const index: number = this.publications.indexOf(publication);
            if (index !== -1) {
                this.publications.splice(index, 1);
            }
        })
    }

    addProject(project) {
        this.httpClient.put(environment.apiUrl + "/api/data/projects", project.toJson()).toPromise().then((newProj: Project) => {
            this.projects.push(new Project(newProj.id, newProj.name, newProj.description));
            this.newProject = new Project()
        });
    }

    editProject(project) {
        project.editable = true
    }

    saveProject(project) {
        this.httpClient.post(environment.apiUrl + '/api/data/projects', project.toJson()).toPromise().then(() => {
            project.editable = false
        })
    }

    deleteProject(project) {
        const options = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            }),
            body: project.toJson()
        };
        this.httpClient.delete(environment.apiUrl + '/api/data/projects', options).toPromise().then(() => {
            const index: number = this.projects.indexOf(project);
            if (index !== -1) {
                this.projects.splice(index, 1);
            }
        })
    }
}
