<template>
  <div class='svg-container'>
    <svg id='chart' style='background-color:yellow'>
    </svg>
  </div>
</template>

<script lang='ts'>
import Vue from 'vue';
import Component from 'vue-class-component';
// @ts-ignore
import * as d3 from 'd3';
import { ForceLink } from 'd3';
import Organism from '../models/Organism';
import Node from '../models/Node';
import ConnectionGene from '../models/ConnectionGene';
import InputNode from '../models/InputNode';
import OutputNode from '../models/OutputNode';
import D3Link from '@/models/D3Link';
import D3Node from '@/models/D3Node';

@Component({
  data: () => ({
    chartWidth: 600,
    chartHeight: 600
  }),
  props: [
    'organism'
  ],
  async mounted() {
    const organism: Organism = this.$props.organism;
    const nodes: D3Node[] = [
      ...organism.inputNodes,
      ...organism.outputNodes,
      ...organism.getHiddenNodes()
      ].map((n) => {
        let color: string = 'black';
        if (n instanceof InputNode) {
          color = 'blue';
        } else if (n instanceof OutputNode) {
          color = 'green';
        }
        return new D3Node(n.nodeIdentifier, color);
      });

    const links: D3Link[] = organism.connectionGenes.map((cg: ConnectionGene) => {
      const source: D3Node = nodes.find((n: D3Node) => n.nodeIdentifier === cg.inNodeIdentifier)!;
      const target: D3Node = nodes.find((n: D3Node) => n.nodeIdentifier === cg.outNodeIdentifier)!;
      return new D3Link(source, target);
    });

    d3.select('.svg-container')
      .style('width', this.$data.chartWidth + 'px')
      .style('height', this.$data.chartHeight + 'px');

    const svg = d3.select('#chart')
      .attr('preserveAspectRatio', 'xMinYMin meet')
      .attr('viewBox', '0 0 ' + this.$data.chartWidth + ' ' + this.$data.chartHeight)
      .attr('transform-origin', '50% 50% 0')
      .classed('svg-content', true);

    const zoom = d3.zoom()
      .scaleExtent([0.5, 5])
      .translateExtent([[0, 0], [this.$data.chartWidth, this.$data.chartHeight]])
      .extent([[0, 0], [this.$data.chartWidth, this.$data.chartHeight]])
      .on('zoom', () => svg.style('transform', 'scale(' + d3.event.transform.k + ')'));

    // @ts-ignore
    svg.call(zoom);

    const simulation = d3.forceSimulation<D3Node, D3Link>()
      .force('link', d3.forceLink<D3Node, D3Link>())
      .force('charge', d3.forceManyBody())
      .force('center', d3.forceCenter(this.$data.chartWidth / 2, this.$data.chartHeight / 2));

    const link = svg.append('g')
      .attr('class', 'links')
      .selectAll('line')
      .data(links)
      .enter()
      .append('line')
        .attr('stroke', 'red');

    const node = svg.append('g')
      .attr('class', 'nodes')
      .selectAll('circle')
      .data(nodes)
      .enter()
      .append('circle')
        .attr('r', 8.5)
        .attr('color', (d) => d.color)
        .call(d3.drag<SVGCircleElement, D3Node, SVGGElement>()
          .on('start', dragstarted)
          .on('drag', dragged)
          .on('end', dragended));

    simulation
      .nodes(nodes)
      .on('tick', ticked);

    simulation.force<ForceLink<D3Node, D3Link>>('link')!
      .links(links);

    function ticked() {
      link
        .attr('x1', (d: D3Link) => d.source.x!)
        .attr('y1', (d: D3Link) => d.source.y!)
        .attr('x2', (d: D3Link) => d.target.x!)
        .attr('y2', (d: D3Link) => d.target.y!);

      node
        .attr('cx', (d: D3Node) => d.x!)
        .attr('cy', (d: D3Node) => d.y!);
    }

    function dragstarted(d: any) {
      if (!d3.event.active) {
        simulation.alphaTarget(0.3).restart();
      }
      d.fx = d.x;
      d.fy = d.y;
    }

    function dragged(d: any) {
      d.fx = d3.event.x;
      d.fy = d3.event.y;
    }

    function dragended(d: any) {
      if (!d3.event.active) {
        simulation.alphaTarget(0);
      }
      d.fx = null;
      d.fy = null;
    }
  }
})
export default class OrganismGraph extends Vue {

}
</script>
<style scoped>
.svg-container {
  display: inline-block;
  position: relative;
  vertical-align: top;
  overflow: hidden;
}

.svg-content {
  display: inline-block;
  position: absolute;
  top: 0;
  left: 0;
}
</style>