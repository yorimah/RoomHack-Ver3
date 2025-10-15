
// where t: class t ～に入れる型を固定する。
public interface State<T>
    where T : class
{
    /// <summary>
    /// このStateになった時に呼ばれる
    /// </summary>
    void Enter(T t);
    /// <summary>
    /// このState中はずっと呼ばれる
    /// </summary>
    void Execute(T t);
    /// <summary>
    /// このStateから変わる時に呼ばれる
    /// </summary>
    void Exit(T t);
}
/// <summary>
/// null避けのためのクラス
/// </summary>
public class NullState<T> : State<T>
    where T : class
{
    public void Enter(T t) { }
    public void Execute(T t) { }
    public void Exit(T t) { }
}

/// <summary>
/// 有限ステートマシン(FSM), where T : class
/// </summary>
public class StateMachine<T>
    where T : class
{
    private T m_Owner;
    /// <summary>
    /// 切り替えるステート
    /// </summary>
    private State<T> m_CurrentState;
    public State<T> currentState { get { return m_CurrentState; } set { m_CurrentState = value; } }
    private State<T> m_PreviousState;
    public State<T> previousState { get { return m_PreviousState; } set { m_PreviousState = value; } }
    /// <summary>
    /// ずっと呼ばれるステート
    /// </summary>
    private State<T> m_GlobalState;
    public State<T> globalState { get { return m_GlobalState; } set { m_GlobalState = value; } }
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public StateMachine(T owner)
    {
        m_Owner = owner;
        m_CurrentState = new NullState<T>();
        m_PreviousState = new NullState<T>();
        m_GlobalState = new NullState<T>();
    }

    /// <summary>
    /// 現在の状態を実行する
    /// </summary>
    public void Update()
    {
        m_GlobalState.Execute(m_Owner);
        m_CurrentState.Execute(m_Owner);
    }
    /// <summary>
    /// 現在のStateを変更する
    /// </summary>
    public void ChangeState(State<T> newState)
    {
        // Assert(newState != null);
        m_PreviousState = m_CurrentState;
        m_CurrentState.Exit(m_Owner);
        m_CurrentState = newState;
        m_CurrentState.Enter(m_Owner);
    }

    /// <summary>
    /// 前のStateに変更する
    /// </summary>
    public void RevertToPreviousState()
    {
        ChangeState(m_PreviousState);
    }
}