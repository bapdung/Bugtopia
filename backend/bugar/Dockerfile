# 빌드 스테이지
FROM gradle:8.10.2-jdk17 AS build
WORKDIR /app
COPY build.gradle settings.gradle ./
COPY gradle ./gradle

# 의존성 다운로드
RUN gradle dependencies --no-daemon

# 소스 코드 복사
COPY src ./src

# 애플리케이션 빌드 및 JAR 파일 생성
RUN gradle clean build -x test --no-daemon

# 실행 스테이지
FROM openjdk:17-jdk-slim
WORKDIR /app

# 빌드 스테이지에서 생성된 JAR 파일을 컨테이너로 복사
COPY --from=build /app/build/libs/bugar-0.0.1-SNAPSHOT.jar ./app.jar

EXPOSE 8080
ENTRYPOINT ["java", "-jar", "app.jar"]