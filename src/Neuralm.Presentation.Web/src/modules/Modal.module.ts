import { Module, MutationTree, ActionTree } from 'vuex';

export interface IModalState {
    showModal: boolean;
    header?: string;
    modalName: string;
    modalPropsData: any;
}

export type setModalPayload = {
    header?: string,
    modalName: string,
    propsData: any
};

export interface IModalMutations extends MutationTree<IModalState> {
    toggleModal(state: IModalState): void;
    setModal(state: IModalState, payload: setModalPayload): void;
}

export interface IModalModule {
    namespaced?: boolean;
    state?: IModalState;
    mutations?: IModalMutations;
}

/**
 * Represents the Modal module class, an implementation of the IModalModule interface.
 */
export default class ModalModule implements IModalModule, Module<IModalState, IModalState> {
    public namespaced?: boolean;
    public state?: IModalState;
    public mutations?: IModalMutations;

    /**
     * Initializes an instance of the ModalModule.
     */
    public constructor() {
        this.namespaced = true;
        this.state = this.getModalState();
        this.mutations = this.getMutations();
    }

    private getModalState(): IModalState {
        const state: IModalState = {
            showModal: false,
            header: '',
            modalName: '',
            modalPropsData: {}
        };
        return state;
    }

    private getMutations(): IModalMutations {
        const mutations: IModalMutations = {
            toggleModal(state: IModalState): void {
                state.showModal = !state.showModal;
            },
            setModal(state: IModalState, payload: setModalPayload): void {
                state.header = payload.header;
                state.modalName = payload.modalName;
                state.modalPropsData = payload.propsData;
            }
        };
        return mutations;
    }
}
