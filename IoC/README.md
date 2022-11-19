# IoC

- DI(dependency injection)는 보다 일반적인 개념 inversion of control(IoC)의 일종; 객체 생성의 제어가 역전된다.

# dependency inversion

- 쉽게 바뀌지 않는 고수준의 정책이, 쉽게 바뀌는 저수준의 디테일한 내용에 의존해서는 안된다 — dependency inversion이 필요하다.
- SOLID principle의 DIP
- 자동차를 보았을 때 우리는 그것의 외적인 모습을 보는 것이지, 엔진 구조를 보는 것이 아니다. 즉, 우리는 클라이언트로서 자동차의 인터페이스를 보게 된다. 따라서 interface는 본질적으로 그것의 client에 가까운 개념이다 — interface는 그 client와 같은 module에 두어야지, implementation이 속해 있는 module과 같이 두어야 할 대상이 아님. 그렇게 하지 않으면 interface를 도입하더라도 dependency inversion이 일어나지 못하게 된다.