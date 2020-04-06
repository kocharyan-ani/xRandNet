import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {ResearchService} from "../../common/research/services/research.service";
import {Observable} from "rxjs";
import {BaseResearch} from "../../common/research/models/base-research";
import {Column} from "../../common/listing/models/Column";

@Component({
    selector: 'app-research-listing',
    templateUrl: './research-listing.component.html',
    styleUrls: ['./research-listing.component.css']
})
export class ResearchListingComponent implements OnInit {

    public researches$: Observable<BaseResearch[]>;
    public columns: Column[];
    public displayColumns: string[];
    public selectedResearch: BaseResearch;

    @Output() select = new EventEmitter<BaseResearch>();

    constructor(private researchService: ResearchService) {
    }

    ngOnInit() {
        this.researches$ = this.researchService.getResearches();
        this.setColumns()
    }

    private setColumns() {
        this.columns = [
            {
                label: 'Research',
                field: 'research'
            },
            {
                label: 'Name',
                field: 'name'
            },
            {
                label: 'Model',
                field: 'model'
            },
            {
                label: 'Storage',
                field: 'storage'
            },
            {
                label: 'Generation',
                field: 'generation'
            },
            {
                label: 'Tracing',
                field: 'tracing'
            },
            {
                label: 'Check Connected',
                field: 'connected'
            },
            {
                label: 'Realizations',
                field: 'count'
            },
            {
                label: 'Status',
                field: 'status'
            }
        ];

        this.displayColumns = this.columns.map(column => column.field)
    }

    public selectResearch(research: BaseResearch) {
        this.selectedResearch = research;
        this.select.emit(this.selectedResearch);
    }
}
