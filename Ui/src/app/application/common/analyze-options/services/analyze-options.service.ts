import {Injectable} from '@angular/core';
import {ResearchEnum} from "../../research/enums/research-enum";
import {AnalyzeOptionEnum} from "../enums/analyze-option-enum";

@Injectable({
    providedIn: 'root'
})
export class AnalyzeOptionsService {

    constructor() {
    }

    public get(research: ResearchEnum): AnalyzeOptionEnum[] {
        switch (research) {
            case ResearchEnum.BASIC:
                return [
                    AnalyzeOptionEnum.AvgPathLength,
                    AnalyzeOptionEnum.Diameter,
                    AnalyzeOptionEnum.AvgDegree,
                    AnalyzeOptionEnum.AvgClusteringCoefficient,
                    AnalyzeOptionEnum.Cycles3,
                    AnalyzeOptionEnum.Cycles4,
                    AnalyzeOptionEnum.EigenDistanceDistribution,
                    AnalyzeOptionEnum.EigenValues,
                    AnalyzeOptionEnum.LaplacianEigenValues,
                    AnalyzeOptionEnum.DegreeDistribution,
                    AnalyzeOptionEnum.ClusteringCoefficientDistribution,
                    AnalyzeOptionEnum.ConnectedComponentDistribution,
                    AnalyzeOptionEnum.ClusteringCoefficientPerVertex,
                    AnalyzeOptionEnum.DistanceDistribution,
                    AnalyzeOptionEnum.TriangleByVertexDistribution,
                    AnalyzeOptionEnum.DegreeCentrality,
                    AnalyzeOptionEnum.ClosenessCentrality,
                    AnalyzeOptionEnum.BetweennessCentrality
                ];
            case ResearchEnum.EVOLUTION:
                return [
                    AnalyzeOptionEnum.Cycles3Evolution
                ];
            case ResearchEnum.THRESHOLD:
                return [
                    AnalyzeOptionEnum.AvgPathLength,
                    AnalyzeOptionEnum.Diameter,
                    AnalyzeOptionEnum.AvgDegree,
                    AnalyzeOptionEnum.AvgClusteringCoefficient,
                    AnalyzeOptionEnum.Cycles3,
                    AnalyzeOptionEnum.Cycles4,
                    AnalyzeOptionEnum.EigenDistanceDistribution,
                    AnalyzeOptionEnum.EigenValues,
                    AnalyzeOptionEnum.LaplacianEigenValues,
                    AnalyzeOptionEnum.DegreeDistribution,
                    AnalyzeOptionEnum.ClusteringCoefficientDistribution,
                    AnalyzeOptionEnum.ConnectedComponentDistribution,
                    AnalyzeOptionEnum.ClusteringCoefficientPerVertex,
                    AnalyzeOptionEnum.DistanceDistribution,
                    AnalyzeOptionEnum.TriangleByVertexDistribution,
                    AnalyzeOptionEnum.DegreeCentrality,
                    AnalyzeOptionEnum.ClosenessCentrality,
                    AnalyzeOptionEnum.BetweennessCentrality
                ];
            case ResearchEnum.COLLECTION:
                return [
                    AnalyzeOptionEnum.Algorithm_1_By_All_Nodes,
                    AnalyzeOptionEnum.Algorithm_2_By_Active_Nodes_List,
                    AnalyzeOptionEnum.Algorithm_Final
                ];
            case ResearchEnum.STRUCTURAL:
                return [
                    AnalyzeOptionEnum.AvgPathLength,
                    AnalyzeOptionEnum.Diameter,
                    AnalyzeOptionEnum.AvgDegree,
                    AnalyzeOptionEnum.AvgClusteringCoefficient,
                    AnalyzeOptionEnum.Cycles3,
                    AnalyzeOptionEnum.Cycles4,
                    AnalyzeOptionEnum.EigenDistanceDistribution,
                    AnalyzeOptionEnum.EigenValues,
                    AnalyzeOptionEnum.LaplacianEigenValues,
                    AnalyzeOptionEnum.DegreeDistribution,
                    AnalyzeOptionEnum.ClusteringCoefficientDistribution,
                    AnalyzeOptionEnum.ConnectedComponentDistribution,
                    AnalyzeOptionEnum.ClusteringCoefficientPerVertex,
                    AnalyzeOptionEnum.DistanceDistribution,
                    AnalyzeOptionEnum.TriangleByVertexDistribution,
                    AnalyzeOptionEnum.DegreeCentrality,
                    AnalyzeOptionEnum.ClosenessCentrality,
                    AnalyzeOptionEnum.BetweennessCentrality
                ];
            case ResearchEnum.ACTIVATION:
                return [
                    AnalyzeOptionEnum.Algorithm_1_By_All_Nodes,
                    AnalyzeOptionEnum.Algorithm_2_By_Active_Nodes_List,
                    AnalyzeOptionEnum.Algorithm_Final
                ]
        }
    }
}
