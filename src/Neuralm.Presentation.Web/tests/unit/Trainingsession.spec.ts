import { Wrapper, mount } from '@vue/test-utils';
import { IMessage } from '@/interfaces/IMessage';
import { IMessageProcessor } from '@/interfaces/IMessageProcessor';
import TrainingSessionView from '@/views/Trainingsession.vue';
import { createMockedNeuralmMQClient, flush, registerPlugins, getLastSnotifyNotification, snotifyNotificationsContains } from '../utilities/TestUtility';
import Vuex from '@/plugins/vuex';
import { IRootState } from '@/interfaces/IRootState';
import NeuralmRouter from '@/plugins/vuerouter';
import Guid from '@/helpers/Guid';
import MessageWrapper from '@/messaging/MessageWrapper';
import messageTypeCache from '@/messaging/MessageTypeCache';
import ITrainingSessionService from '@/interfaces/ITrainingSessionService';
import TrainingSessionService from '@/services/TrainingSessionService';
import TrainingSessionModule, { ITrainingSessionModule } from '@/modules/TrainingSession.module';
import Organism from '@/models/Organism';
import GetOrganismsResponse from '@/messages/responses/GetOrganismsResponse';
import User from '@/models/User';
import Vuetify from 'vuetify';

jest.useFakeTimers();
registerPlugins();

describe('Trainingsession.vue', () => {
  let wrapper: Wrapper<TrainingSessionView>;
  let messages: IMessage[];
  let messageProcessor: IMessageProcessor;
  let trainingSessionModule: ITrainingSessionModule;
  let trainingSessionId: string;
  let userId: string;

  beforeEach(async () => {
    messages = [];
    localStorage.clear();
    userId = Guid.newGuid().toString();
    const user: User = { username: 'Mario', accessToken: 'token', userId };
    localStorage.setItem('user', JSON.stringify(user));
    const { processor, mq } = createMockedNeuralmMQClient(messages);
    messageProcessor = processor;
    const trainingSessionService: ITrainingSessionService = new TrainingSessionService(mq);
    trainingSessionModule = new TrainingSessionModule();
    trainingSessionId = Guid.newGuid().toString();
    trainingSessionModule.state!.trainingSession = {
      trainingRoomId: Guid.newGuid().toString(),
      id: trainingSessionId,
      endedTimestamp: '0001-01-01T00:00:00',
      startedTimestamp: new Date().toDateString(),
      userId
    };
    trainingSessionModule.state!.name = 'Cool room';

    const store = new Vuex.Store<IRootState>({
      modules: {
        trainingSession: trainingSessionModule
      }
    });
    const router = new NeuralmRouter([
      { name: 'home' },
      { name: 'trainingsession', props: { trainingSessionService } }
    ], true);
    // NOTE: to fix <v-data-table> from throwing errors manually add a vuetify instance
    const vuetify = new Vuetify();
    wrapper = mount(TrainingSessionView,
      {
        store,
        router,
        vuetify,
        propsData: {
          trainingSessionService
        },
        sync: false
      });
    wrapper.vm.$router.push('/trainingsession');
    wrapper.vm.$snotify.clear();
    await flush();
  });

  it('Faulty network structure should fail to construct when testing Xor', async () => {
    await flush();
    wrapper.find('button.getOrganisms').trigger('click');
    await flush();
    const orgs: Organism[] = JSON.parse('[{"id":"1110df35-52f0-4bb5-8ce4-c68de3f50717","connectionGenes":[{"id":"ab1f8a88-f22c-49ce-e69c-08d7a435cc56","organismId":"1110df35-52f0-4bb5-8ce4-c68de3f50717","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":0.9468057444071425,"enabled":true},{"id":"608c8dba-cb98-4e0d-e69d-08d7a435cc56","organismId":"1110df35-52f0-4bb5-8ce4-c68de3f50717","inNodeIdentifier":1,"outNodeIdentifier":2,"weight":0.8501794032054857,"enabled":true}],"inputNodes":[{"id":"23b86cf7-9feb-4e83-9b66-52ee08f002bb","layer":0,"nodeIdentifier":1},{"id":"b29a8f7e-e9a9-4bea-b177-c41556650edd","layer":0,"nodeIdentifier":0}],"outputNodes":[{"id":"cbf2e2d9-edb9-4a22-be06-fc7c81a08d9b","layer":0,"nodeIdentifier":2}],"score":0,"name":"hah","generation":2},{"id":"5f58a628-d6f9-4c89-894c-cc0cc67b4b58","connectionGenes":[{"id":"5e10a17e-d5ed-4f43-e6be-08d7a435cc56","organismId":"5f58a628-d6f9-4c89-894c-cc0cc67b4b58","inNodeIdentifier":1,"outNodeIdentifier":2,"weight":0.8874416885839037,"enabled":true},{"id":"2ce5cf89-401f-45f4-e6bf-08d7a435cc56","organismId":"5f58a628-d6f9-4c89-894c-cc0cc67b4b58","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":0.9468057444071425,"enabled":true}],"inputNodes":[{"id":"04263382-c8a1-4d9b-ad8d-78ebd11d1634","layer":0,"nodeIdentifier":1},{"id":"8f09d844-d41b-4aef-b518-bbb3b81ea4d1","layer":0,"nodeIdentifier":0}],"outputNodes":[{"id":"d9b4add4-8a76-4c13-91e4-30187428af54","layer":0,"nodeIdentifier":2}],"score":0,"name":"huv","generation":2},{"id":"233ca2a9-016a-4165-9f04-d0cd30599466","connectionGenes":[{"id":"de88f87c-a903-4d91-e6c6-08d7a435cc56","organismId":"233ca2a9-016a-4165-9f04-d0cd30599466","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":-0.8599432335979972,"enabled":true},{"id":"13c275df-3ca7-4ad7-e6c7-08d7a435cc56","organismId":"233ca2a9-016a-4165-9f04-d0cd30599466","inNodeIdentifier":1,"outNodeIdentifier":2,"weight":0.9279808662496418,"enabled":true}],"inputNodes":[{"id":"aab6743b-bd78-4601-8f44-36ced34d525c","layer":0,"nodeIdentifier":0},{"id":"6c38d04f-568d-4ed4-9030-652c5fdb3f0b","layer":0,"nodeIdentifier":1}],"outputNodes":[{"id":"f7a8d5ee-2833-4c80-9454-6ffdf5586b9c","layer":0,"nodeIdentifier":2}],"score":0,"name":"meev","generation":2},{"id":"a100859d-5c5f-4b32-bc4b-cf0f65744add","connectionGenes":[{"id":"84660026-932a-4d41-e6b8-08d7a435cc56","organismId":"a100859d-5c5f-4b32-bc4b-cf0f65744add","inNodeIdentifier":1,"outNodeIdentifier":2,"weight":0.5511769361566645,"enabled":true},{"id":"8937ed99-3ea7-4fe0-e6b9-08d7a435cc56","organismId":"a100859d-5c5f-4b32-bc4b-cf0f65744add","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":-0.6720582465045425,"enabled":true}],"inputNodes":[{"id":"8f7a517c-39dc-421b-9718-163eb7c90d89","layer":0,"nodeIdentifier":0},{"id":"ddc31fdb-cd5a-47b9-a256-7d43aaf7cb3e","layer":0,"nodeIdentifier":1}],"outputNodes":[{"id":"7428049a-dd70-4430-8ec1-6a3687d2814a","layer":0,"nodeIdentifier":2}],"score":0,"name":"wopoom","generation":2},{"id":"879c8309-4542-4882-9ae0-f49aaee1ded1","connectionGenes":[{"id":"08544f7b-eafb-4d78-e6b0-08d7a435cc56","organismId":"879c8309-4542-4882-9ae0-f49aaee1ded1","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":-0.911530219908585,"enabled":true},{"id":"8a42569d-1f19-421f-e6b1-08d7a435cc56","organismId":"879c8309-4542-4882-9ae0-f49aaee1ded1","inNodeIdentifier":1,"outNodeIdentifier":2,"weight":0.5396867401617051,"enabled":true}],"inputNodes":[{"id":"ecf569b1-9cc1-46b2-a070-27462634b1ba","layer":0,"nodeIdentifier":1},{"id":"cc5293ae-d3e2-41af-8d5f-e23472e10638","layer":0,"nodeIdentifier":0}],"outputNodes":[{"id":"80461bd3-e8e6-4150-a38c-c78ec6a3b0ff","layer":0,"nodeIdentifier":2}],"score":0,"name":"kixityc","generation":2},{"id":"dd44e9ea-6e1c-42f2-a713-de9f60f36e0f","connectionGenes":[{"id":"70978ca8-01a0-4ad3-e6ca-08d7a435cc56","organismId":"dd44e9ea-6e1c-42f2-a713-de9f60f36e0f","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":0.3642462377269968,"enabled":true},{"id":"269e36a6-4185-421c-e6cb-08d7a435cc56","organismId":"dd44e9ea-6e1c-42f2-a713-de9f60f36e0f","inNodeIdentifier":1,"outNodeIdentifier":2,"weight":0.8933658557913107,"enabled":true}],"inputNodes":[{"id":"cf286c7e-df70-4f20-9705-81b37a71790b","layer":0,"nodeIdentifier":0},{"id":"c65a7bc1-d0d4-4b1f-a495-f87cbadcf847","layer":0,"nodeIdentifier":1}],"outputNodes":[{"id":"c988fd4d-e05b-40a0-8e32-78f19fc5f751","layer":0,"nodeIdentifier":2}],"score":0,"name":"haafeezaab","generation":2},{"id":"e9e90ef8-5c77-4c4f-9214-eeecbc890cc5","connectionGenes":[{"id":"fcbf70b3-6255-448e-e6aa-08d7a435cc56","organismId":"e9e90ef8-5c77-4c4f-9214-eeecbc890cc5","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":0.7500601238804219,"enabled":true},{"id":"1646d659-0670-41cc-e6ab-08d7a435cc56","organismId":"e9e90ef8-5c77-4c4f-9214-eeecbc890cc5","inNodeIdentifier":1,"outNodeIdentifier":2,"weight":0.9267026316498884,"enabled":true}],"inputNodes":[{"id":"39ee22e9-1be8-44cf-bf2c-9334b4741a57","layer":0,"nodeIdentifier":0},{"id":"45ea4559-3f38-48bd-b337-e3105225573e","layer":0,"nodeIdentifier":1}],"outputNodes":[{"id":"0051c8eb-ef42-4852-bc57-b2897cc9d73b","layer":0,"nodeIdentifier":2}],"score":0,"name":"cous","generation":2},{"id":"cc95b2f0-b498-4291-b9fd-d6d1654b7459","connectionGenes":[{"id":"9c738b6b-7efd-480d-e6a6-08d7a435cc56","organismId":"cc95b2f0-b498-4291-b9fd-d6d1654b7459","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":0.8646167163572352,"enabled":true},{"id":"3702122f-1df3-4761-e6a7-08d7a435cc56","organismId":"cc95b2f0-b498-4291-b9fd-d6d1654b7459","inNodeIdentifier":1,"outNodeIdentifier":2,"weight":0.8874416885839037,"enabled":true}],"inputNodes":[{"id":"1a4c974f-f4f3-4bab-bcba-675e0c222d44","layer":0,"nodeIdentifier":1},{"id":"c028f4b5-3475-46a9-bf6d-d4ba999a9728","layer":0,"nodeIdentifier":0}],"outputNodes":[{"id":"985cc945-e4bd-44f1-b45c-bc14cb0af9f4","layer":0,"nodeIdentifier":2}],"score":0,"name":"gan","generation":2},{"id":"9704e91d-ae1e-4ff7-9a02-c67d4f86182f","connectionGenes":[{"id":"1c65a50e-2c21-439e-e6ac-08d7a435cc56","organismId":"9704e91d-ae1e-4ff7-9a02-c67d4f86182f","inNodeIdentifier":1,"outNodeIdentifier":2,"weight":-0.8779935286743537,"enabled":false},{"id":"b9e266da-932b-4eeb-e6ad-08d7a435cc56","organismId":"9704e91d-ae1e-4ff7-9a02-c67d4f86182f","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":0.3642462377269968,"enabled":true},{"id":"7f27e42d-778e-4fba-e6ae-08d7a435cc56","organismId":"9704e91d-ae1e-4ff7-9a02-c67d4f86182f","inNodeIdentifier":1,"outNodeIdentifier":0,"weight":1,"enabled":true},{"id":"68d79c3c-cf40-438b-e6af-08d7a435cc56","organismId":"9704e91d-ae1e-4ff7-9a02-c67d4f86182f","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":-0.8672354359027163,"enabled":true}],"inputNodes":[{"id":"cccf3da0-cf42-4fb6-9749-660eb011359a","layer":0,"nodeIdentifier":1},{"id":"06b715ea-adaa-40e3-8e7a-7f1dbecd069b","layer":0,"nodeIdentifier":0}],"outputNodes":[{"id":"0a7901ff-2a01-44be-adc9-8ffca05d8000","layer":0,"nodeIdentifier":2}],"score":0,"name":"hyz","generation":2},{"id":"cb6d22f5-4d6c-4add-81ad-d7f518634fcc","connectionGenes":[{"id":"7474d31c-073d-4b39-e69b-08d7a435cc56","organismId":"cb6d22f5-4d6c-4add-81ad-d7f518634fcc","inNodeIdentifier":0,"outNodeIdentifier":2,"weight":-0.36319920670390093,"enabled":true}],"inputNodes":[{"id":"cc2f4aaa-79f1-40f5-bef2-4f8788269440","layer":0,"nodeIdentifier":0},{"id":"3c2f4a2d-767a-4807-af5f-a40a3104dc64","layer":0,"nodeIdentifier":1}],"outputNodes":[{"id":"1b106372-4f1a-4200-9ada-4a3c5bc39a25","layer":0,"nodeIdentifier":2}],"score":0,"name":"piebacocauv","generation":2}]');
    const response: GetOrganismsResponse = new GetOrganismsResponse(
      Guid.newGuid().toString(),
      messages[0].id,
      new Date(),
      'The requested amount of organisms are not all available. The training room is close to a new generation.',
      true,
      orgs
    );
    const messageWrapper: MessageWrapper = {
      name: messageTypeCache.getMessageType('GetOrganismsResponse'),
      message: response
    };
    messageProcessor.processMessage(messageWrapper);
    await flush();
    wrapper.find('button.testOrganisms').trigger('click');
    await flush();
    expect(snotifyNotificationsContains(wrapper.vm, 'Faulty network structure, failed to construct network!')).toBe(true);
  });
});
