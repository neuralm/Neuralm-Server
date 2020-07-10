import * as d3 from 'd3';
import D3Node from '@/models/D3Node';

/**
 * Represents the D3Link class.
 * Used to create forced edge networks.
 */
export default class D3Link implements d3.SimulationLinkDatum<D3Node> {
    constructor(
        public source: D3Node,
        public target: D3Node
    ) { }
}
