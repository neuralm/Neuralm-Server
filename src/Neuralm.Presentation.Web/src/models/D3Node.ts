import * as d3 from 'd3';

/**
 * Represents the D3Node class.
 * Used to create forced edge networks.
 */
export default class D3Node implements d3.SimulationNodeDatum {
    public x?: number;
    public y?: number;

    constructor(
        public nodeIdentifier: number,
        public color: string
    ) { }
}
