//*****************************************************************************
//** 1514. Path with Maximum Probability  leetcode                           **
//*****************************************************************************


#define MAX_NODES 5000

typedef struct {
    int node;
    double prob;
} Pair;

typedef struct {
    Pair *edges;
    int size;
    int capacity;
} GraphNode;

typedef struct {
    int items[MAX_NODES];
    int front;
    int rear;
    int size;
} Queue;

void initQueue(Queue *q) {
    q->front = 0;
    q->rear = -1;
    q->size = 0;
}

bool isEmpty(Queue *q) {
    return q->size == 0;
}

void enqueue(Queue *q, int item) {
    if (q->size == MAX_NODES) return; // queue is full
    q->rear = (q->rear + 1) % MAX_NODES;
    q->items[q->rear] = item;
    q->size++;
}

int dequeue(Queue *q) {
    if (isEmpty(q)) return -1;
    int item = q->items[q->front];
    q->front = (q->front + 1) % MAX_NODES;
    q->size--;
    return item;
}

void addEdge(GraphNode *graph, int u, int v, double prob) {
    if (graph[u].size == graph[u].capacity) {
        graph[u].capacity *= 2;
        graph[u].edges = realloc(graph[u].edges, graph[u].capacity * sizeof(Pair));
    }
    graph[u].edges[graph[u].size].node = v;
    graph[u].edges[graph[u].size].prob = prob;
    graph[u].size++;
}

double maxProbability(int n, int** edges, int edgesSize, int* edgesColSize, double* succProb, int succProbSize, int start_node, int end_node) {
    GraphNode *graph = malloc(n * sizeof(GraphNode));
    for (int i = 0; i < n; i++) {
        graph[i].edges = malloc(2 * sizeof(Pair));
        graph[i].size = 0;
        graph[i].capacity = 2;
    }

    for (int i = 0; i < edgesSize; i++) {
        int u = edges[i][0], v = edges[i][1];
        double prob = succProb[i];
        addEdge(graph, u, v, prob);
        addEdge(graph, v, u, prob);
    }

    double *d = calloc(n, sizeof(double));
    bool *vis = calloc(n, sizeof(bool));
    d[start_node] = 1.0;

    Queue q;
    initQueue(&q);
    enqueue(&q, start_node);
    vis[start_node] = true;

    while (!isEmpty(&q)) {
        int i = dequeue(&q);
        vis[i] = false;

        for (int k = 0; k < graph[i].size; k++) {
            int j = graph[i].edges[k].node;
            double s = graph[i].edges[k].prob;
            if (d[j] < d[i] * s) {
                d[j] = d[i] * s;
                if (!vis[j]) {
                    enqueue(&q, j);
                    vis[j] = true;
                }
            }
        }
    }

    double result = d[end_node];
    
    // Clean up memory
    for (int i = 0; i < n; i++) {
        free(graph[i].edges);
    }
    free(graph);
    free(d);
    free(vis);
    
    return result;
}